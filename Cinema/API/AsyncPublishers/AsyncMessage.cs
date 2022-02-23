namespace API.AsyncPublishers
{
    public class AsyncMessage
    {
        public string Type { get; set; }
        public string Body { get; set; }

        public AsyncMessage(string type, string body)
        {
            Type = type;
            Body = body;
        }
    }
}
