using AddressParser.Guessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AddressParserTest
{
    [TestClass]
    public class CountryGuesserTest
    {
        #region TryGuessCountry
        [TestMethod]
        public void TryGuessCountryTest()
        {
            var countries = CountryGuesser.Countries;
            var result = CountryGuesser.TryGuessCountry("ch");

            Assert.AreEqual("CH", result);
        }
        #endregion

        #region GetLocalizedCountryNameFromISOCode
        [TestMethod]
        public void GetLocalizedCountryNameFromISOCodeTest()
        {
            var countries = CountryGuesser.Countries;
            var result = CountryGuesser.GetLocalizedCountryNameFromISOCode("ch");

            Assert.AreEqual("Switzerland", result);
        } 
        #endregion
    }
}
