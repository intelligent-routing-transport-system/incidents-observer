using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace incidents_observer.Kafka
{
    public class ProducerMessage<T> : IDisposable
    {
        T Value { get; set; }
        string Topic { get; set; }
        string ConnectionString { get; set; }

        public ProducerMessage(T value, string topic, string connectionString)
        {
            Value = value;
            Topic = topic;
            ConnectionString = connectionString;
        }
        public async Task<string> Run()
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = ConnectionString,
                Acks = Acks.All
            };

            var p = new ProducerBuilder<string, T>(config).Build();
            var dr = await p.ProduceAsync(Topic, new Message<string, T>
            {
                Value = Value,
                Key = Guid.NewGuid().ToString()
            });

            return $"Mensagem enviada com sucesso: {dr.Value} | Partition: {dr.Partition} | {dr.Topic}";
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
