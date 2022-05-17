using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Reservation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; } = DateTime.Now;

        public bool HasEnded { get; set; }

        [ForeignKey("Bed")]
        public long BedId { get; set; }
        public Bed Bed { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("ReservationRequest")]
        public long ReservationRequestId { get; set; }
        public ReservationRequest ReservationRequest { get; set; }
    }
}
