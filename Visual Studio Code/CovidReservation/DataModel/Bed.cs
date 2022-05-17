using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Bed
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Number { get; set; }

        [MaxLength(500)]
        public string Status { get; set; }

        [Required]
        public bool IsActive { get; set; } = false;

        [ForeignKey("Room")]
        public string RoomId { get; set; }

        public Room Room { get; set; }

        public List<Reservation> Reservations { get; set; }

        [NotMapped]
        public string Discription {
            get
            {
                return (Room!=null ? Room.Discription + "-" : "") + Number;
            }
        }
    }
}
