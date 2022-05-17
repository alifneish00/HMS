using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataModel
{
    public class User
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string Mobile { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(200)]
        //[DataType(DataType.Password)]
        public string Password { get; set; } = "123456";

        public bool IsAdmin { get; set; } = false;

        public List<Reservation> Reservations { get; set; }

        [ForeignKey("Hospital")]
        public string HospitalId { get; set; }

        [JsonIgnore]
        public virtual Hospital Hospital { get; set; }

    }
}
