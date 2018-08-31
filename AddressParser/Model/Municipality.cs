using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressParser.Model
{
    [Serializable]
    public class Municipality
    {
        public string DisplayName => string.Format(CultureInfo.CurrentCulture, "{0} {1}", MunicipalityNumber, MunicipalityName);

        public string MunicipalityName { get; set; }

        public string MunicipalityNumber { get; set; }

        public string Canton { get; set; }
    }
}
