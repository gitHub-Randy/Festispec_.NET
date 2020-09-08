using FestiSpec.PdfBuilder.Styling.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Node
{
    public class PdfHeaderNode : PdfTextNode
    {
        public PdfHeaderNode(string text) : base(text)
        {
            Size = new FontSizeTag("30px");
            Weight = new FontWeightTag(FontWeightTag.Values.bold);
        }
    }
}
