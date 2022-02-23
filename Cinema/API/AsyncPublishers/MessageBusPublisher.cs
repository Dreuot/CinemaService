using API.AsyncPublishers;
using API.AsyncPublishers.Params;
using Microsoft.VisualStudio.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace API.AsyncPublishers
{
    public class MessageBusPublisher : IMessageBusPublisher
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IBasicProperties _props;
        private readonly BlockingCollection<string> responceQueue = new BlockingCollection<string>();
        private readonly AsyncQueue<string> asyncResponseQueue = new AsyncQueue<string>();

        public MessageBusPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
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

            _channel.QueueDeclare("request", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare("response", durable: false, exclusive: false, autoDelete: false, arguments: null);

            _props = _channel.CreateBasicProperties();
            _props.ReplyTo = "response";
            _props.CorrelationId = Guid.NewGuid().ToString();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.Default.GetString(body);
                if (ea.BasicProperties.CorrelationId == _props.CorrelationId)
                {
                    responceQueue.Add(response);
                    asyncResponseQueue.Enqueue(response);
                }
            };

            _channel.BasicConsume("response", autoAck: false, consumer);
        }

        public string GetAllMovies()
        {
            SendMessage(nameof(GetAllMovies), string.Empty);

            string response = string.Empty;
            if (responceQueue.TryTake(out response, 5000))
            {
                return response;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<string> GetAllMoviesAsync()
        {
            SendMessage(nameof(GetAllMovies), string.Empty);

            var source = new CancellationTokenSource();
            source.CancelAfter(5000);
            CancellationToken token = source.Token;

            string response = null;
            try
            {
                response = await asyncResponseQueue.DequeueAsync();
            }
            catch(OperationCanceledException e)
            {
            }

            return response;
        }

        public async Task<string> GetCinemaMovieSeances(int cinemaId, int movieId)
        {
            GetCinemaMovieSeances @params = new GetCinemaMovieSeances(cinemaId, movieId);
            SendMessage(nameof(GetCinemaMovieSeances), JsonSerializer.Serialize(@params));

            var source = new CancellationTokenSource();
            source.CancelAfter(5000);
            CancellationToken token = source.Token;

            string response = null;
            try
            {
                response = await asyncResponseQueue.DequeueAsync();
            }
            catch (OperationCanceledException e)
            {
            }

            return response;
        }

        private void SendMessage(string type, string message)
        {

            AsyncMessage asyncMessage = new AsyncMessage(type, message);
            byte[] body = Encoding.Default.GetBytes(JsonSerializer.Serialize(asyncMessage));

            _channel.BasicPublish(
                exchange: "",
                routingKey: "request",
                basicProperties: _props,
                body: body
            );
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
