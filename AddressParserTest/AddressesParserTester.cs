using AddressParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AddressParserTest
{
    [TestClass]
    public class AddressesParserTester
    {
        [TestMethod]
        public void Parser_ParseAddress()
        {
            var addressParser = AddressesParser.Parse("Lidstostrasse 5\r\n6006 Luzern");

            Assert.AreEqual("6006", addressParser.PostalCode);
            Assert.AreEqual("Luzern", addressParser.City);
            Assert.AreEqual("Lidstostrasse 5", addressParser.Street);
        }

        [TestMethod]
        public void Parser_ParseAddress_WithoutBackslashR()
        {
            var addressParser = AddressesParser.Parse("Lidstostrasse 5\n6006 Luzern");

            Assert.AreEqual("6006", addressParser.PostalCode);
            Assert.AreEqual("Luzern", addressParser.City);
            Assert.AreEqual("Lidstostrasse 5", addressParser.Street);
        }

        [TestMethod]
        public void Parser_ParseAddressWithPoBox()
        {
            var addressParser = AddressesParser.Parse("Lidstostrasse 5\r\nPostfach 42\r\n6006 Luzern");

            Assert.AreEqual("6006", addressParser.PostalCode);
            Assert.AreEqual("Luzern", addressParser.City);
            //Assert.AreEqual("Postfach 42", addressParser.PostalBox);
            Assert.AreEqual("Lidstostrasse 5", addressParser.Street);
        }

        [TestMethod]
        public void Parser_ParseAddressWithCountryCode()
        {
            var addressParser = AddressesParser.Parse("Lidstostrasse 5\r\nCH-6006 Luzern");

            Assert.AreEqual("6006", addressParser.PostalCode);
            Assert.AreEqual("Luzern", addressParser.City);
            Assert.AreEqual("Lidstostrasse 5", addressParser.Street);
        }

        [TestMethod]
        public void Parser_ParseWithLongerCityName()
        {
            var addressParser = AddressesParser.Parse("Bul Oslobodjenja 38\r\n21000 Novi Sad");

            Assert.AreEqual("21000", addressParser.PostalCode);
            Assert.AreEqual("Novi Sad", addressParser.City);
            Assert.AreEqual("Bul Oslobodjenja 38", addressParser.Street);
        }

        [TestMethod]
        public void Parser_ParseWithoutPostalCodeOrCity()
        {
            var addressParser = AddressesParser.Parse("Bul Oslobodjenja 38\r\nNovi Sad");

            Assert.IsNull(addressParser.PostalCode);
            Assert.IsNull(addressParser.City);

            addressParser = AddressesParser.Parse("Bul Oslobodjenja 38\r\n21000");

            Assert.AreEqual("21000", addressParser.PostalCode);
            Assert.IsTrue(string.IsNullOrEmpty(addressParser.City));
        }

        [TestMethod]
        public void Parser_ParseWithCityNameInFirstRow()
        {
            var addressParser = AddressesParser.Parse("Bul Oslobodjenja 38\r\n21000 Novi Sad\r\nSerbia");

            Assert.AreEqual("21000", addressParser.PostalCode);
            Assert.AreEqual("Novi Sad", addressParser.City);
            Assert.AreEqual("Bul Oslobodjenja 38", addressParser.Street);
            Assert.AreEqual("Serbia", addressParser.Country);
        }

        [TestMethod]
        public void Parser_ParseWithMultilineAddress()
        {
            var addressParser = AddressesParser.Parse("IT Engine\r\nBul Oslobodjenja 38\r\n21000 Novi Sad\r\nSerbia");

            Assert.AreEqual("21000", addressParser.PostalCode);
            Assert.AreEqual("Novi Sad", addressParser.City);
            Assert.AreEqual("Bul Oslobodjenja 38", addressParser.Street);
            Assert.AreEqual("Serbia", addressParser.Country);

            var addressLines = addressParser.AddressLines;
            Assert.AreEqual("IT Engine", addressLines[0]);
            Assert.AreEqual("Bul Oslobodjenja 38", addressLines[1]);
            Assert.AreEqual("Serbia", addressLines[2]);
        }
    }
}
