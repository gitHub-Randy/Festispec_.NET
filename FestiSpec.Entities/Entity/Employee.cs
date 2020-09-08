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
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(45)]
        public string Name { get; set; }

        [Required]
        [MaxLength(45)]
        public string TelephoneNumber { get; set; }

        [Required]
        [MaxLength(45)]
        public string Email { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        [MaxLength(6)]
        public string HouseNumber { get; set; }

        [Required]
        [MaxLength(6)]
        public string ZipCode { get; set; }

        public bool IsArchived { get; set; } = false;

        [Required]
        public virtual Role Role { get; set; }

        public virtual EmployeeAccount Account { get; set; }

        public virtual ICollection<EmployeeAbsence> AbsencesDates { get; set; }
    }
}
