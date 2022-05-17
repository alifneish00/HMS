using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Room
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string Number { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int Floor { get; set; } = 0;

        [Required]
        public bool IsActive { get; set; } = true;

        [ForeignKey("Hospital")]
        public string HospitalId { get; set; }

        public Hospital Hospital { get; set; }

        public List<Bed> Beds { get; set; }

        [NotMapped]
        public string Discription
        {
            get
            {
                return (Hospital != null ? Hospital.Name + "-" : "") + Number;
            }
        }
    }
}
