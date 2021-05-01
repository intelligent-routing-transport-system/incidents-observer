using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace incidents_observer.Models
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        [Required]
        public string IdIncident { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double ActionRadius { get; set; }

        [Required]
        public bool BlockPoint { get; set; }

        [Required]
        public string IdSensor { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
