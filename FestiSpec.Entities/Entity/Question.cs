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
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string QuestionText { get; set; }
        
        public string Description { get; set; }

        [Required]
        public int Index { get; set; }

        [MaxLength(300)]
        public string QuestionOptions { get; set; }

        [MaxLength(45)]
        public string Type { get; set; }

        //Navigation properties
        public virtual QuestionType QuestionType { get; set; }

        public virtual QuestionList QuestionList { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
