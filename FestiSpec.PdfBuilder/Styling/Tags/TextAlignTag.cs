using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Styling.Tags
{
    public class TextAlignTag : IStyleTag
    {
        public Values Alignment { get; set; }

        public TextAlignTag(Values align)
        {
            Alignment = align;
        }

        public string ToStyleTag()
        {
            return String.Format("text-align:{0};", Alignment);
        }

        public enum Values
        {
            left,
            center,
            right
        }
    }
}
