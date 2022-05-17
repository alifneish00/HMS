using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class ReservationRequest
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public ReservationRequestStatus Status { get; set; } = ReservationRequestStatus.Pending;

        [Required]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Hospital")]
        public string HospitalId { get; set; }
        public Hospital Hospital { get; set; }

        [ForeignKey("Reservation")]
        
        public long? ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }

        [NotMapped]
        public string Discription {
            get
            {
                return (User != null ? User.Name + "-" : "") + ReservationRequestStatus.Accepted;
            }
        }
    }

}
