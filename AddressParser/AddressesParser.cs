using AddressParser.Guessing;
using AddressParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AddressParser
{
    public class AddressesParser
    {
        private static IEnumerable<string> SplitAddress(string formattedAddress) => formattedAddress?.Split(Environment.NewLine.ToCharArray())?.Where(addressLine => !string.IsNullOrEmpty(addressLine.Trim()))?.ToList() ?? new List<string>();

        private static void SetSwitzerlandAsCountryBy(Address address)
        {
            var cantonGessed = CantonGuesser.TryGuessCanton(address.PostalCode, address.City);
            if (!string.IsNullOrEmpty(cantonGessed)) address.Country = CountryGuesser.GetLocalizedCountryNameFromISOCode("CH");
        }

        public static Address Parse(string address)
        {
            var addressParsed = new Address();
            var postalCodeReg = new Regex(@"(?<canton>[A-Za-z]{0,3})[\-\s]+(?<postalcode>[0-9]{4,6})\s(?<city>.*)$");

            if (!string.IsNullOrEmpty(address))
            {
                var splittedAddress = SplitAddress(address);

                var match = postalCodeReg.Match(address);
                if (match.Success)
                {
                    addressParsed.PostalCode = match.Groups["postalcode"].Value;
                    addressParsed.City = match.Groups["city"].Value;
                    SetSwitzerlandAsCountryBy(addressParsed);
                }

                var zipRegex = new Regex("(?<postalCode>[0-9]{4,6})");
                var streetRegex = new Regex("(?<street>[A-za-z]*[0-9]{1,3})");

                foreach (var line in splittedAddress)
                {
                    var zipMatch = zipRegex.Match(line);
                    var streetMatch = streetRegex.Match(line);
                    var countryMatch = CountryGuesser.TryGuessCountry(line.Trim());

                    if (zipMatch.Success && addressParsed.City == null && addressParsed.PostalCode == null)
                    {
                        addressParsed.PostalCode = zipMatch.Groups["postalCode"].Value;
                        addressParsed.City = line.Replace(addressParsed.PostalCode, string.Empty).Trim(',', ' ');
                    }
                    else if (streetMatch.Success && addressParsed.Street == null) addressParsed.Street = line.Trim();

                    if (!zipMatch.Success && !streetMatch.Success && !string.IsNullOrEmpty(countryMatch)) addressParsed.Country = countryMatch;
                }

                addressParsed.AddressLines.Clear();
                if (splittedAddress.Any(addressLine => (addressParsed.City != null && addressLine.Contains(addressParsed.City)) && (addressParsed.PostalCode != null && addressLine.Contains(addressParsed.PostalCode))))
                {
                    SetSwitzerlandAsCountryBy(addressParsed);
                    foreach (var addressLine in splittedAddress)
                    {
                        if (addressLine.Contains(addressParsed.City) && addressLine.Contains(addressParsed.PostalCode)) continue;
                        addressParsed.AddressLines.Add(addressLine);
                    }
                }
            }

            return addressParsed;
        }
    }
}
