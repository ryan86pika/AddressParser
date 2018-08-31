using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressParser.Model
{
    [Serializable]
    public class Address
    {
        private List<string> _addressLines;

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Street { get; set; }

        public string PostalBox { get; set; }

        public IList<string> AddressLines => _addressLines ?? (_addressLines = new List<string>());
    }
}
