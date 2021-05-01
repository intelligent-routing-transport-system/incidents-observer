using Confluent.Kafka;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace incidents_observer.Kafka
{
    public class ConsumerMessage
    {
        string Topic { get; set; }
        string GroupId { get; set; }
        string ConnectionString { get; set; }

        public ConsumerMessage(string topic, string groupId, string connectionString)
        {
            Topic = topic;
            GroupId = groupId;
            ConnectionString = connectionString;
        }

        public string Run()
        {
            string message = null;
            ConsumerConfig conf = new ConsumerConfig
            {
                GroupId = GroupId,
                BootstrapServers = ConnectionString,
            };

            using (var c = new ConsumerBuilder<Ignore, byte[]>(conf).Build())
            {
                c.Subscribe(Topic);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    try
                    {
                        var retorno = c.Consume(cts.Token).Value;
                        message = Encoding.UTF8.GetString(retorno);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
            return message;
        }
    }
}
