using NUnit.Framework;
using UnitConverter.Services;
using System;

namespace UnitConverter.Tests
{
    [TestFixture]
    public class ConversionServiceTests
    {
        [TestCase(1000, "mm", "m", ExpectedResult = 1)]
        [TestCase(100, "cm", "m", ExpectedResult = 1)]
        [TestCase(1, "m", "km", ExpectedResult = 0.001)]
        [TestCase(1, "inch", "m", ExpectedResult = 0.0254)]
        [TestCase(1, "foot", "m", ExpectedResult = 0.3048)]
        [TestCase(1, "yard", "m", ExpectedResult = 0.9144)]
        [TestCase(1, "mile", "m", ExpectedResult = 1609.34)]
        public double ConvertLength_ValidConversions(double value, string fromUnit, string toUnit)
        {
            return ConversionService.ConvertLength(value, fromUnit, toUnit);
        }

        [TestCase(1000, "mg", "g", ExpectedResult = 1)]
        [TestCase(1000, "g", "kg", ExpectedResult = 1)]
        [TestCase(1, "kg", "lb", ExpectedResult = 2.20462)]
        [TestCase(1, "ounce", "g", ExpectedResult = 28.3495)]
        [TestCase(1, "lb", "kg", ExpectedResult = 0.453592)]
        public double ConvertWeight_ValidConversions(double value, string fromUnit, string toUnit)
        {
            return ConversionService.ConvertWeight(value, fromUnit, toUnit);
        }

        [TestCase(0, "celsius", "fahrenheit", ExpectedResult = 32)]
        [TestCase(100, "celsius", "fahrenheit", ExpectedResult = 212)]
        [TestCase(0, "celsius", "kelvin", ExpectedResult = 273.15)]
        [TestCase(32, "fahrenheit", "celsius", ExpectedResult = 0)]
        [TestCase(212, "fahrenheit", "celsius", ExpectedResult = 100)]
        [TestCase(32, "fahrenheit", "kelvin", ExpectedResult = 273.15)]
        [TestCase(273.15, "kelvin", "celsius", ExpectedResult = 0)]
        [TestCase(373.15, "kelvin", "celsius", ExpectedResult = 100)]
        [TestCase(273.15, "kelvin", "fahrenheit", ExpectedResult = 32)]
        public double ConvertTemperature_ValidConversions(double value, string fromUnit, string toUnit)
        {
            return ConversionService.ConvertTemperature(value, fromUnit, toUnit);
        }

        [Test]
        public void ConvertLength_InvalidConversion_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ConversionService.ConvertLength(1, "invalid", "m"));
        }

        [Test]
        public void ConvertWeight_InvalidConversion_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ConversionService.ConvertWeight(1, "invalid", "kg"));
        }

        [Test]
        public void ConvertTemperature_InvalidConversion_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ConversionService.ConvertTemperature(1, "invalid", "celsius"));
        }
    }
}
