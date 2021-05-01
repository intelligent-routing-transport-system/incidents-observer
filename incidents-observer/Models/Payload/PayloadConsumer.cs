using System.ComponentModel.DataAnnotations.Schema;

namespace incidents_observer.Models.Payload
{
    [NotMapped]
    public class PayloadConsumer
    {
        public class Coord
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
        public Coord Coords { get; set; }
        public double Value { get; set; }
        public double ValueToCompare { get; set; }
        public double ActionRadius { get; set; }
        public string SensorId { get; set; }
    }
}
