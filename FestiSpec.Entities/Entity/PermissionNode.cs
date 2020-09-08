using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Entity
{
    [Serializable]
    public class PermissionNode
    {
        [Key]
        public string Permission { get; set; }
        
        //Navigation properties
        public virtual ICollection<Role> Roles { get; set; }
    }
}
