using BackService.AsyncConsumers;

namespace BackService.EventProcessings
{
    public interface IAsyncEventProcessor
    {
        public string ProcessEvent(AsyncMessage message);
    }
}
