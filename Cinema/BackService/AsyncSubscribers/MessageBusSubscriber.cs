using BackService.EventProcessings;
using BackService.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BackService.AsyncConsumers
{
    public class MessageBusSubscriber : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private IModel _channel;
        private IConnection _connection;

        public MessageBusSubscriber(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;

            int port;
            if (!int.TryParse(_configuration["RabbitMQPort"], out port))
                port = 5672;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"] ?? "localhost",
                Port = port,
                UserName = _configuration["RabbitMQUser"] ?? "guest",
                Password = _configuration["RabbitMQPassword"] ?? "guest",
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "request",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _channel.QueueDeclare(queue: "response",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _channel.BasicQos(0, 1, false);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnReceived;

            _channel.BasicConsume(queue: "request", 
                autoAck: false, 
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        private void OnReceived(object? sender, BasicDeliverEventArgs args)
        {
            IBasicProperties props = args.BasicProperties;
            IBasicProperties replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            string requestBody = Encoding.Default.GetString(args.Body.ToArray());
            byte[] response = null;
            try
            {
                AsyncMessage request = JsonSerializer.Deserialize<AsyncMessage>(requestBody);

                IAsyncEventProcessor processor = AsyncEventProcessor.GetProcessor(request, _scopeFactory);
                var processed = processor.ProcessEvent(request);
                response = Encoding.Default.GetBytes(processed);
                //var unicodeStr = Encoding.Unicode.GetBytes(processed);
                //response = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, unicodeStr);
            }
            catch
            {
                response = null;
            }

            _channel.BasicPublish(exchange: "",
                routingKey: props.ReplyTo,
                basicProperties: replyProps,
                body: response
            );

            _channel.BasicAck(args.DeliveryTag, false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            return Task.CompletedTask;
        }
    }
}
