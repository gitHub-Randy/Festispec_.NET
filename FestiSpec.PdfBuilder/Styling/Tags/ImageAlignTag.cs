using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Styling.Tags
{
    public class ImageAlignTag : IStyleTag
    {
        public Values Alignment { get; set; }

        public ImageAlignTag(Values align)
        {
            Alignment = align;
        }

        public string ToStyleTag()
        {
            if (Alignment.Equals(Values.center))
                return "display:block; margin-left:auto; margin-right:auto;";
            if (Alignment.Equals(Values.left))
                return "display:block; margin-left:0px; margin-right:auto;";
            if (Alignment.Equals(Values.right))
                return "display:block; margin-left:auto; margin-right:0px;";
            return "";
        }

        public enum Values
        {
            left,
            center,
            right
        }
    }
}
