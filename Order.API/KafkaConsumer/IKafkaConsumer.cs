namespace Order.API.KafkaConsumer
{
    public interface IKafkaConsumer
    {
        public void ConsumeMessages(CancellationToken cancellationToken);
        public void RunInBackground();


    }
}
