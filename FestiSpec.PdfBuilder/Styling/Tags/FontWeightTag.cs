using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Styling.Tags
{
    public class FontWeightTag : IStyleTag
    {
        public Values Weight { get; set; }

        public FontWeightTag(Values align)
        {
            Weight = align;
        }

        public string ToStyleTag()
        {
            return String.Format("font-weight:{0};", Weight);
        }

        public enum Values
        {
            lighter,
            normal,
            bold,
            bolder
        }
    }
}
