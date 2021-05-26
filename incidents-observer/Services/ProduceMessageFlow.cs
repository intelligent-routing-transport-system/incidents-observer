using incidents_observer.Kafka;
using System;
using System.Diagnostics;
using System.Text.Json;
using incidents_observer.Models.Payload;
using Microsoft.Extensions.Configuration;
using incidents_observer.Models.Configurations;
using System.Threading.Tasks;
using incidents_observer.Repository.UnityOfWork;
using incidents_observer.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace incidents_observer.Services
{
    public class ProduceMessageFlow
    {
        private readonly IUnityOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly PayloadConsumer _payloadConsumer;
        public ProduceMessageFlow(IConfiguration configuration, IUnityOfWork uof, PayloadConsumer payloadConsumer)
        {
            _uof = uof;
            _configuration = configuration;
            _payloadConsumer = payloadConsumer;
        }

        private KafkaProducerConfiguration GetConfiguration()
        {
            return new KafkaProducerConfiguration()
            {
                TopicName = _configuration.GetValue<string>("KafkaProducerConfiguration:TopicName"),
                KafkaConnectionString = _configuration.GetConnectionString("KafkaConnection"),
            };
        }
        public async Task<PayloadProducer> Run()
        {
            try
            {
                var messagaDatabase = await _uof.MessageRepository.
                    Get()
                    .Where(x => x.IdSensor == _payloadConsumer.SensorId)
                    .FirstOrDefaultAsync();

                var payloadProducer = new PayloadProducer()
                {
                    Coords = new PayloadProducer.Coord { Latitude = _payloadConsumer.Coords.Latitude, Longitude = _payloadConsumer.Coords.Longitude },
                    ActionRadius = _payloadConsumer.ActionRadius,
                    BlockPoint = (_payloadConsumer.Value >= _payloadConsumer.ValueToCompare),
                };

                if (messagaDatabase is not null)
                {
                    payloadProducer.IdIncident = messagaDatabase.IdIncident;
                    messagaDatabase.BlockPoint = payloadProducer.BlockPoint;
                    _uof.MessageRepository.Update(messagaDatabase);
                    await _uof.Commit();
                }

                else
                {
                    var messageCreated = new Message()
                    {
                        IdIncident = Guid.NewGuid().ToString(),
                        ActionRadius = payloadProducer.ActionRadius,
                        BlockPoint = payloadProducer.BlockPoint,
                        Latitude = payloadProducer.Coords.Latitude,
                        Longitude = payloadProducer.Coords.Longitude,
                        IdSensor = _payloadConsumer.SensorId,
                        CreatedAt = DateTime.Now
                    };

                    _uof.MessageRepository.Add(messageCreated);
                    await _uof.Commit();

                    payloadProducer.IdIncident = messageCreated.IdIncident;
                }

                var messageToSend = JsonSerializer.SerializeToUtf8Bytes(payloadProducer);
                var kafkaConfigurations = GetConfiguration();
                using (var producer = new ProducerMessage<byte[]>(messageToSend, kafkaConfigurations.TopicName, kafkaConfigurations.KafkaConnectionString))
                {
                    var messageSend = await producer.Run();
                    Debug.WriteLine(messageSend);
                }
                Debug.WriteLine("Health Check");
                return payloadProducer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
