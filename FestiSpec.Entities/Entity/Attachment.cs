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
    public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FilePath { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
