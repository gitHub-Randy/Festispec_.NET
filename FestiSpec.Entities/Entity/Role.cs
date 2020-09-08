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
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        //Navigation properties
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<PermissionNode> PermissionNodes { get; set; }
    }
}
