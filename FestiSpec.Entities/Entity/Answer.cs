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
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string AnswerText { get; set; }

        [Required]
        public virtual Inspection Inspection { get; set; }
        
        [Required]
        public virtual Question Question { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
