using FestiSpec.PdfBuilder.Styling.Tags;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Node
{
    public class PdfImageBase64Node : IPdfNode
    {
        public string Base64 { get; set; }
        public WidthTag Width { get; set; }
        public HeightTag Height { get; set; }
        public ImageAlignTag Alignment { get; set; }

        public PdfImageBase64Node(string base64)
        {
            Base64 = base64;
            Width = new WidthTag("auto");
            Height = new HeightTag("auto");
            Alignment = new ImageAlignTag(ImageAlignTag.Values.center);
        }

        public PdfImageBase64Node(string base64, string height)
        {
            Base64 = base64;
            Height = new HeightTag("auto");
            Width = new WidthTag(height);
            Alignment = new ImageAlignTag(ImageAlignTag.Values.center);
        }

        public PdfImageBase64Node(string base64, string width, string height)
        {
            Base64 = base64;
            Width = new WidthTag(width);
            Height = new HeightTag(height);
            
        }

        public PdfImageBase64Node(string base64, string width, string height, ImageAlignTag.Values align)
        {
            Base64 = base64;
            Width = new WidthTag(width);
            Height = new HeightTag(height);
            Alignment = new ImageAlignTag(align);
        }

        public string ToHTML()
        {
            var imageURI = @"data:image/png;base64," + Base64;
            return string.Format("<img {0} src='{1}'>", GetStyling(), imageURI);
        }

        public string GetStyling()
        {
            return string.Format("style=\"{0} {1}\"", Width.ToStyleTag(), Height.ToStyleTag());
        }
    }
}
