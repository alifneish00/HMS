using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Hospital
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string Latitude { get; set; }
        
        [MaxLength(100)]
        public string Longitude { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }

        public List<Room> Rooms { get; set; }

        public List<User> Users { get; set; }

        public List<ReservationRequest> ReservationRequests { get; set; }
    }
}
