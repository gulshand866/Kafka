using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Order.API.Controllers;
using Order.API.IRepo;
using Order.API.Models;
using System.Threading;

namespace Order.API.KafkaConsumer
{
    public class KafkaConsumers : IKafkaConsumer
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly string _topic;

        private readonly KafkaSettings _kafkaSettings;


        public KafkaConsumers(IOptions<KafkaSettings> kafkaSettings, IServiceScopeFactory serviceScopeFactory)
        {
            _kafkaSettings = kafkaSettings.Value;

            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = _kafkaSettings.SaslUsername,
                SaslPassword = _kafkaSettings.SaslPassword,
                GroupId = $"consumer-group-1"
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            _topic = _kafkaSettings.Topic ?? throw new Exception("Kafka Topic is null. Check your configuration.");

            _serviceScopeFactory = serviceScopeFactory;

            _consumer.Subscribe(_topic);
        }

        public void ConsumeMessages(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _orderRepo = scope.ServiceProvider.GetService<IOrderRepo>();
                if (_orderRepo != null)
                {
                    try
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                var consumeResult = _consumer.Consume(cancellationToken);

                                if (consumeResult != null && consumeResult.Message != null)
                                {
                                    dynamic? deserializedMessage = JsonConvert.DeserializeObject(consumeResult.Message.Value);


                                    // Process or handle the deserialized message as needed
                                    Console.WriteLine($"consumeResult = {consumeResult}");
                                    Console.WriteLine($"consumeResult.Message = {consumeResult.Message}");
                                    Console.WriteLine($"consumeResult.Message.Value = {consumeResult.Message.Value}");
                                    Console.WriteLine($"deserializedMessage = {deserializedMessage}");

                                    if(deserializedMessage != null )
                                    {

                                        var order = new Orders 
                                        {
                                            ProductId = deserializedMessage.Id,
                                            NumberOfItems = deserializedMessage.Quantity,
                                            TotalPrice = deserializedMessage.Quantity * deserializedMessage.Price,
                                        };

                                        _orderRepo.CreateOrder(order);

                                    }

                                    _consumer.Commit();
                                }
                            }
                            catch (ConsumeException e)
                            {
                                // Handle consume exceptions
                                Console.WriteLine($"Error while consuming: {e.Error.Reason}");
                            }
                        }
                    }
                    finally
                    {
                        _consumer.Close();
                    }
                }
            }
        }


        public void RunInBackground()
        {
            CancellationToken cancellationToken = new CancellationToken();
            Task.Run(() => ConsumeMessages(cancellationToken));
        }
    }
}
