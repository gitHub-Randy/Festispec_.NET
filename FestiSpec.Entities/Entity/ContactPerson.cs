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
    public class ContactPerson
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

        //Navigation properties
        public virtual ICollection<CustomerCompany> Companies { get; set; }
    }
}
