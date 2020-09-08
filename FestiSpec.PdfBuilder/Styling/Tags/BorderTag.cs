using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Styling.Tags
{
    public class BorderTag : IStyleTag
    {
        public StyleValues Style { get; set; }
        public int Width { get; set; }

        public BorderTag(StyleValues style, int width)
        {
            Style = style;
            Width = width;
        }

        public enum StyleValues
        {
            dotted,
            dashed,
            solid,
            groove,
            ridge,
            inset,
            outset,
            none,
            hidden,
            doubleBorder
        }

        public string ToStyleTag()
        {
            return String.Format("border-style: {0}; border-width: {1}; ", Style.ToString().Replace("Border", ""), Width);
        }
    }
}
