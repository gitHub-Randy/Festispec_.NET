using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestiSpec.PdfBuilder;
using FestiSpec.PdfBuilder.Node;
using FestiSpec.PdfBuilder.Styling.Tags;

namespace FestiSpec.PdfBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Builder builder = new Builder();
            builder
                .AddHeader("Gekke dingen")
                .AddText("Cool XD")
                .AddFooter("(c) Martijn");

            foreach (BorderTag.StyleValues style in (BorderTag.StyleValues[])Enum.GetValues(typeof(BorderTag.StyleValues)))
            {
                builder.AddNode(new PdfTextNode("This has a border :D") { Border = new BorderTag(style, 1)});
            }

            builder.Export("file.pdf");
        }
    }
}
