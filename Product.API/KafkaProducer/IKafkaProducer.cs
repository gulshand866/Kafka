namespace Product.API.KafkaProducer
{
    public interface IKafkaProducer
    {
        public Task ProduceMessage(object message);
    }
}
