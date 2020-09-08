using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Node
{
    public class PdfHtmlNode : IPdfNode
    {
        public string Html { get; set; }

        public PdfHtmlNode(string html)
        {
            Html = html;
        }

        public string ToHTML()
        {
            return Html;
        }

        public string GetStyling()
        {
            return "";
        }
    }
}
