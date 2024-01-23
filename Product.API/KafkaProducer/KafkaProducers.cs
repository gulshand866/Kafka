using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Product.API.KafkaProducer
{
    public class KafkaProducers : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        private readonly KafkaSettings _kafkaSettings;

        public KafkaProducers(IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings.Value;

            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = _kafkaSettings.SaslUsername,
                SaslPassword = _kafkaSettings.SaslPassword
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();

            _topic = _kafkaSettings.Topic ?? throw new Exception("Kafka Topic is null. Check your configuration.");

        }

        public async Task ProduceMessage(object message)
        {
            var serializedMessage = JsonConvert.SerializeObject(message);
            var response = await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = serializedMessage });
        }
    }
}
