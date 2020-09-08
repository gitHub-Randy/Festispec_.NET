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
    public class Inspection
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Present { get; set; }

        public string ReasonsAbsent { get; set; }
        
        //Navigation properties
        [Required]
        public virtual Employee Employee { get; set; }

        [Required]
        public virtual QuestionList QuestionList { get; set; }
    }
}
