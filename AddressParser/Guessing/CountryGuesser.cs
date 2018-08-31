using AddressParser.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AddressParser.Guessing
{
    public static class CountryGuesser
    {
        private static IEnumerable<JsonCountry> _countries;
        public static IEnumerable<JsonCountry> Countries
        {
            get
            {
                if (_countries == null)
                {
                    // I get this json from https://www.npmjs.com/package/world-countries
                    var resourceStream = typeof(CountryGuesser).Assembly.GetManifestResourceStream("AddressParser.Resources.countries.json");

                    using (var sr = new StreamReader(resourceStream))
                    {
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            var serializer = new JsonSerializer();
                            _countries = serializer.Deserialize<JsonCountry[]>(jsonTextReader);
                        }
                    }
                }

                return _countries;
            }
        }

        public static string GetLocalizedCountryNameFromISOCode(string ISOCode2ch)
        {
            var language = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
            var country = Countries.FirstOrDefault(f => f.CCA2.ToUpper().Equals(ISOCode2ch.ToUpper()));
            if (language.Equals("eng")) return country.Name.Common;
            else if (country.Name.Native.ContainsKey(language)) return country.Name.Native[language].Common;
            else if (country.Translations.ContainsKey(language)) return country.Translations[language].Common;
            else return country.CCA2;
        }

        public static string TryGuessCountry(string countryString)
        {
            foreach (var country in Countries)
            {
                foreach (var native in country.Name.Native)
                {
                    if (countryString.Contains(native.Value.Common)) return native.Value.Common;
                    else if (countryString.Contains(native.Value.Official)) return native.Value.Official;
                }

                if (countryString.Contains(country.Name.Common)) return country.Name.Common;
                else if (countryString.Contains(country.Name.Official)) return country.Name.Official;

                foreach (var native in country.Translations)
                {
                    if (countryString.Contains(Regex.Unescape(native.Value.Common))) return Regex.Unescape(native.Value.Common);
                    else if (countryString.Contains(Regex.Unescape(native.Value.Official))) return Regex.Unescape(native.Value.Official);
                }

                if (countryString.ToUpper().Equals(country.CCA2)) return country.CCA2;
                else if (countryString.ToUpper().Equals(country.CCA3)) return country.CCA3;
            }

            return string.Empty;
        }
    }
}
