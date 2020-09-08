using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Entity
{
    [Serializable]
    public class QuestionType
    {
        [Key]
        [MaxLength(45)]
        public string Type { get; set; }

        public string Description { get; set; }
    }
}
