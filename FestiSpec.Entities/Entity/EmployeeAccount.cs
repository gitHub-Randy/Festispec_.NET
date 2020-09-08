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
    public class EmployeeAccount
    {
        [Key]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(45)]
        public string Username { get; set; }

        [Required]
        [MaxLength(45)]
        public string Password { get; set; }

        //Navigation properties
        [Required]
        public virtual Employee Employee {get; set;} 
    }
}
