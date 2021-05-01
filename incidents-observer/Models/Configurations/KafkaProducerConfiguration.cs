using System.ComponentModel.DataAnnotations.Schema;

namespace incidents_observer.Models.Configurations
{
    [NotMapped]
    public class KafkaProducerConfiguration
    {
        public string TopicName { get; set; }

        public string KafkaConnectionString { get; set; }
    }
}
