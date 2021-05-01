using System.ComponentModel.DataAnnotations.Schema;

namespace incidents_observer.Models.Payload
{
    [NotMapped]
    public class PayloadProducer
    {
        public class Coord
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
        public Coord Coords { get; set; }

        public bool BlockPoint { get; set; }

        public string IdIncident { get; set; }

        public double ActionRadius { get; set; }
    }
}
