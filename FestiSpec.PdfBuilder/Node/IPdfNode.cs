using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.PdfBuilder.Node
{
    public interface IPdfNode
    {
        string ToHTML();

        string GetStyling();
    }
}
