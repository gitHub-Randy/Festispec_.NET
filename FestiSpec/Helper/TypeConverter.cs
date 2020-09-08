using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Helper
{
    public class TypeConverter
    {
        public static Dictionary<string, string> type = new Dictionary<string, string>
        {
            {"Antwoord per uur", "Table"},
            {"Bijlage", "Image2" },
            {"Open vraag", "Text"},
            {"Slider", "Slider"},
            {"Meerkeuzevragen", "MultipleChoice"},
            {"Ja of Nee", "YesNo"},
            { "Tekening", "Image"},
            { "Tabel", "Table"}
        };
    }
}
