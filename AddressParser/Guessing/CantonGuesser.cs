using AddressParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace AddressParser.Guessing
{
    public static class CantonGuesser
    {
        private static IEnumerable<XmlNode> _cities;
        private static IEnumerable<XmlNode> _municipalities;

        public static IEnumerable<XmlNode> Cities
        {
            get
            {
                if (_cities == null) LoadCity();
                return _cities;
            }
        }

        public static IEnumerable<XmlNode> Municipalities
        {
            get
            {
                if (_municipalities == null) LoadMunicipalities();
                return _municipalities;
            }
        }

        private static void LoadCity()
        {
            XmlDocument doc = new XmlDocument();

            Stream resourceStream = typeof(CantonGuesser).Assembly.GetManifestResourceStream("AddressParser.Resources.citiesInSwitzerland.xml");

            using (resourceStream)
            {
                doc.Load(resourceStream);

                _cities = doc.GetElementsByTagName("City").Cast<XmlNode>();
            }
        }

        private static void LoadMunicipalities()
        {
            XmlDocument doc = new XmlDocument();

            Stream resourceStream = typeof(CantonGuesser).Assembly.GetManifestResourceStream("AddressParser.Resources.municipalitiesInSwitzerland.xml");

            if (resourceStream != null) doc.Load(resourceStream);

            _municipalities = doc.GetElementsByTagName("Municipality").Cast<XmlNode>();
        }

        public static IEnumerable<string> GetAllCantons() => Cities.Select(c => c.Attributes["Canton"].Value).Distinct().ToList();

        public static IEnumerable<Municipality> GetAllMunicipalities()
        {
            return Municipalities
                .Select(c => new Municipality
                {
                    Canton = c.Attributes["CantonNumber"].Value,
                    MunicipalityName = c.Attributes["MunicipalityName"].Value,
                    MunicipalityNumber = c.Attributes["MunicipalityNumber"].Value
                })
            .ToList();
        }

        public static string TryGuessCanton(string formattedAddress)
        {
            Address addressParser = AddressesParser.Parse(formattedAddress);
            return TryGuessCanton(addressParser.PostalCode, addressParser.City);
        }

        public static string TryGuessCanton(string postalCode, string city)
        {
            if (!string.IsNullOrWhiteSpace(postalCode))
            {
                //Try to guess the canton using the postal code
                XmlNode node = Cities.FirstOrDefault(n => n.Attributes["PostalCode"].Value == postalCode);

                //if an element with the given postal code is found return the corresponding canton
                return node?.Attributes["Canton"]?.Value;
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                //else try to guess the canton using the city
                XmlNode node = Cities.FirstOrDefault(n => string.Equals(n.Attributes["City"].Value, city, StringComparison.OrdinalIgnoreCase));
                return node?.Attributes["Canton"]?.Value;
            }

            return string.Empty;
        }

        public static Municipality TryGuessMunicipality(string formattedAddress)
        {
            Address addressParser = AddressesParser.Parse(formattedAddress);

            if (!string.IsNullOrWhiteSpace(addressParser.City))
            {
                //Try to guess the canton using the postal code
                XmlNode node = _municipalities.FirstOrDefault(n => string.Equals(n.Attributes["MunicipalityName"].Value, addressParser.City, StringComparison.OrdinalIgnoreCase));

                //if an element with the given postal code is found return the corresponding canton
                if (node != null)
                {
                    return new Municipality
                    {
                        Canton = node.Attributes["CantonNumber"].Value,
                        MunicipalityName = node.Attributes["MunicipalityName"].Value,
                        MunicipalityNumber = node.Attributes["MunicipalityNumber"].Value
                    };
                }
            }

            return null;

        }
    }
}
