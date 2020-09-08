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
    public class Festival
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(45)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }
        
        public string HouseNumber { get; set; }
        
        public string LocationComment { get; set; }

        [Required]
        public bool IsArchived { get; set; } = false;
        
        [Required]
        public virtual CustomerCompany CustomerCompany { get; set; }

        public virtual ICollection<Inspection> Inspections { get; set; }

        public virtual ICollection<QuestionList> QuestionLists { get; set; }
    }
}