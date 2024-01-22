using Confluent.Kafka;
using Newtonsoft.Json;

namespace Product.API.KafkaProducer
{
    public class KafkaProducers : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducers(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = configuration["Kafka:SaslUsername"],
                SaslPassword = configuration["Kafka:SaslPassword"]
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();

            _topic = configuration["Kafka:Topic"] ?? throw new Exception("Kafka Topic is null. Check your configuration.");

        }

        public async Task ProduceMessage(object message)
        {
            var serializedMessage = JsonConvert.SerializeObject(message);
            var response = await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = serializedMessage });
        }
    }
}
