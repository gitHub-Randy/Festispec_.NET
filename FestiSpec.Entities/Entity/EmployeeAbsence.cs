using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Entity
{
    [Serializable]
    public class EmployeeAbsence
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public DateTime Date { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
