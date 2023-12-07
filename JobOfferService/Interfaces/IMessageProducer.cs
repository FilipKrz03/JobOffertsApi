namespace JobOfferService.Interfaces
{
    public interface IMessageProducer<T>
    {
        void SendMessage(T message);
        void CloseConnection();
    }
}
