using FestiSpec.PdfBuilder.Node;
using FestiSpec.PdfBuilder.Styling.Tags;
using IronPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder
{
    public class Builder
    {
        private List<IPdfNode> _nodes;
        public IronPdf.HtmlToPdf Renderer;

        public Builder()
        {
            _nodes = new List<IPdfNode>();
            Renderer = new IronPdf.HtmlToPdf();
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
        }

        public Builder AddNode(IPdfNode node)
        {
            _nodes.Add(node);
            return this;
        }

        public Builder AddHeader(string text)
        {
            AddNode(new PdfHeaderNode(text));
            return this;
        }

        public Builder AddText(string text)
        {
            AddNode(new PdfTextNode(text));
            return this;
        }

        public Builder AddText(string text, string size)
        {
            AddNode(new PdfTextNode(text, size));
            return this;
        }

        public Builder AddText(string text, string size, FontWeightTag.Values weight)
        {
            AddNode(new PdfTextNode(text, size, weight, TextAlignTag.Values.center));
            return this;
        }

        public Builder AddText(string text, string size, FontWeightTag.Values weight, TextAlignTag.Values alignment)
        {
            AddNode(new PdfTextNode(text, size, weight, alignment));
            return this;
        }

        public Builder AddFooter(string text)
        {
            AddNode(new PdfFooterNode(text));
            return this;
        }

        public Builder AddImage(string filepath)
        {
            AddNode(new PdfImageNode(filepath));
            return this;
        }

        public Builder AddImage(string filepath, string width, string height)
        {
            AddNode(new PdfImageNode(filepath, width, height));
            return this;
        }

        public Builder AddImageBase64(string base64)
        {
            AddNode(new PdfImageBase64Node(base64));
            return this;
        }

        public Builder AddImageBase64(string base64, string height)
        {
            AddNode(new PdfImageBase64Node(base64, height));
            return this;
        }

        public Builder AddImageBase64(string base64, string width, string height)
        {
            AddNode(new PdfImageBase64Node(base64, width, height));
            return this;
        }

        public string BuildHTML()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            foreach (IPdfNode node in _nodes)
            {
                htmlBuilder.Append(node.ToHTML());
                htmlBuilder.Append("\n");
            }
            return htmlBuilder.ToString();
        }

        public void Export(string filepath)
        {            
            string html = BuildHTML();
            var pdfdoc = Renderer.RenderHtmlAsPdf(html);
            pdfdoc.SaveAs(filepath);
        }
    }
}
