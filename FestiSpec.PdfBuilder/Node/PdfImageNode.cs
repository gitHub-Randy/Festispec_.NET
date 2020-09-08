using FestiSpec.PdfBuilder.Styling.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Node
{
    public class PdfImageNode : IPdfNode
    {
        public string FilePath { get; set; }
        public WidthTag Width { get; set; }
        public HeightTag Height { get; set; }
        public ImageAlignTag Alignment { get; set; }

        public PdfImageNode(string filepath)
        {
            FilePath = filepath;
            Width = new WidthTag("auto");
            Height = new HeightTag("auto");
            Alignment = new ImageAlignTag(ImageAlignTag.Values.center);
        }

        public PdfImageNode(string filepath, string width, string height)
        {
            FilePath = filepath;
            Width = new WidthTag(width);
            Height = new HeightTag(height);
            Alignment = new ImageAlignTag(ImageAlignTag.Values.center);
        }

        public PdfImageNode(string filepath, string width, string height, ImageAlignTag.Values align)
        {
            FilePath = filepath;
            Width = new WidthTag(width);
            Height = new HeightTag(height);
            Alignment = new ImageAlignTag(align);
        }

        public string ToHTML()
        {
            var imageBinary = File.ReadAllBytes(FilePath);
            var imageURI = @"data:image/png;base64," + Convert.ToBase64String(imageBinary);
            return String.Format("<img {0} src='{1}'>", GetStyling(), imageURI);
        }

        public string GetStyling()
        {
            return String.Format("style=\"{0} {1} {2}\"", Width.ToStyleTag(), Height.ToStyleTag(), Alignment.ToStyleTag());
        }
    }
}
