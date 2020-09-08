using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Styling.Tags
{
    public class FontSizeTag : IStyleTag
    {
        public string Size { get; set; }

        public FontSizeTag(string size)
        {
            Size = size;
        }

        public string ToStyleTag()
        {
            return String.Format("font-size:{0};", Size);
        }
    }
}
