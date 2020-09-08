using FestiSpec.PdfBuilder.Styling.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Node
{
    public class PdfTextNode : IPdfNode
    {
        public string Text { get; set; }
        public FontWeightTag Weight { get; set; }
        public TextAlignTag Align { get; set; }
        public FontSizeTag Size { get; set; }
        public BorderTag Border { get; set; }

        public PdfTextNode(string text)
        {
            Text = text;
            Weight = new FontWeightTag(FontWeightTag.Values.normal);
            Align = new TextAlignTag(TextAlignTag.Values.left);
            Size = new FontSizeTag("18px");
           // Border = new BorderTag(BorderTag.StyleValues.none, 2);
        }

        public PdfTextNode(string text, string size)
        {
            Text = text;
            Weight = new FontWeightTag(FontWeightTag.Values.normal);
            Align = new TextAlignTag(TextAlignTag.Values.left);
            Size = new FontSizeTag(size);
            Border = new BorderTag(BorderTag.StyleValues.none, 2);
        }

        public PdfTextNode(string text, string size, FontWeightTag.Values weight, TextAlignTag.Values align)
        {
            Text = text;
            Weight = new FontWeightTag(weight);
            Align = new TextAlignTag(align);
            Size = new FontSizeTag(size);
            Border = new BorderTag(BorderTag.StyleValues.none, 2);
        }

        public string ToHTML()
        {
            return String.Format("<p {0}>{1}</p>", GetStyling(), Text);
        }

        public string GetStyling()
        {
            return String.Format("style=\"{0} {1} {2}\"", Size.ToStyleTag(), Weight.ToStyleTag(), Align.ToStyleTag());
        }
    }
}
