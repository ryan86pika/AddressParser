using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressParser.Model
{
    [Serializable]
    public class JsonCountryCommonType
    {
        // common name
        public string Official { get; set; }

        // official name
        public string Common { get; set; }
    }

    [Serializable]
    public class JsonCountryName
    {
        // common name in english
        public string Official { get; set; }

        // official name in english
        public string Common { get; set; }

        //list of all native names
        public IReadOnlyDictionary<string, JsonCountryCommonType> Native { get; set; }
    }

    [Serializable]
    public class JsonCountry
    {
        public JsonCountryName Name { get; set; }

        // ISO code with 2 chars
        public string CCA2 { get; set; }

        // ISO code with 3 chars
        public string CCA3 { get; set; }

        public string[] Currency { get; set; }

        public string[] CallingCode { get; set; }

        // list of official languages
        public IReadOnlyDictionary<string, string> Languages { get; set; }

        // list of name translations
        public IReadOnlyDictionary<string, JsonCountryCommonType> Translations { get; set; }
    }
}
