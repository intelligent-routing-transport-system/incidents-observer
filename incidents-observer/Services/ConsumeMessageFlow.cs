using incidents_observer.Kafka;
using incidents_observer.Models.Payload;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using incidents_observer.Models.Configurations;
using System.Text.Json;

namespace incidents_observer.Services
{
    public class ConsumeMessageFlow
    {
        private readonly IConfiguration _configuration;

        public ConsumeMessageFlow(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private KafkaConsumerConfiguration GetConfigurations()
        {
            return new KafkaConsumerConfiguration()
            {
                TopicName = _configuration.GetValue<string>("KafkaConsumerConfiguration:TopicName"),
                GroupId = _configuration.GetValue<string>("KafkaConsumerConfiguration:GroupId"),
                KafkaConnectionString = _configuration.GetConnectionString("KafkaConnection"),
            };
        }

        public PayloadConsumer Run()
        {
            try
            {
                var kafkaConfigurations = GetConfigurations();
                while (true)
                {
                    var consumer = new ConsumerMessage(kafkaConfigurations.TopicName, kafkaConfigurations.TopicName, kafkaConfigurations.KafkaConnectionString);
                    string message = consumer.Run();

                    if (message is not null)
                    {
                        var json = JsonSerializer.Deserialize<PayloadConsumer>(message);
                        Debug.WriteLine($"Messagem consumida - {json}");
                        return json;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
