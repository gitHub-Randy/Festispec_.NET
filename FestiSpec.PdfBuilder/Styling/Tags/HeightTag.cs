using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Styling.Tags
{
    public class HeightTag : IStyleTag
    {
        public string Height { get; set; }

        public HeightTag(string height)
        {
            Height = height;
        }

        public string ToStyleTag()
        {
            return String.Format("height:{0};", Height);
        }
    }
}
