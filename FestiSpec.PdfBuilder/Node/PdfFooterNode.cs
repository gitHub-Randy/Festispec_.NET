using FestiSpec.PdfBuilder.Styling.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Node
{
    class PdfFooterNode : PdfTextNode
    {
        public PdfFooterNode(string text) : base(text)
        {
            Size = new FontSizeTag("12px");
        }
    }
}
