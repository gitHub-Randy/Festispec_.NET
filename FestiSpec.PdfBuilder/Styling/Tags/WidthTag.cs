using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Styling.Tags
{
    public class WidthTag : IStyleTag
    {
        public string Width { get; set; }

        public WidthTag(string width)
        {
            Width = width;
        }

        public string ToStyleTag()
        {
            return String.Format("width:{0};", Width);
        }
    }
}
