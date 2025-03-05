namespace UnitConverter.Services
{
    public class ConversionService
    {
        private static readonly Dictionary<string, double> LengthConversions = new()
        {
            { "mm-m", 0.001 }, { "cm-m", 0.01 }, { "m-km", 0.001 }, { "inch-m", 0.0254 },
            { "foot-m", 0.3048 }, { "yard-m", 0.9144 }, { "mile-m", 1609.34 }
        };

        private static readonly Dictionary<string, double> WeightConversions = new()
        {
            { "mg-g", 0.001 }, { "g-kg", 0.001 }, { "kg-lb", 2.20462 }, { "ounce-g", 28.3495 },
            { "lb-kg", 0.453592 }
        };

        public static double ConvertLength(double value, string fromUnit, string toUnit)
        {
            if (fromUnit == toUnit) return value;
            return LengthConversions.TryGetValue($"{fromUnit}-{toUnit}", out double factor) ? value * factor
                 : LengthConversions.TryGetValue($"{toUnit}-{fromUnit}", out factor) ? value / factor
                 : throw new ArgumentException("Invalid length conversion.");
        }

        public static double ConvertWeight(double value, string fromUnit, string toUnit)
        {
            if (fromUnit == toUnit) return value;
            return WeightConversions.TryGetValue($"{fromUnit}-{toUnit}", out double factor) ? value * factor
                 : WeightConversions.TryGetValue($"{toUnit}-{fromUnit}", out factor) ? value / factor
                 : throw new ArgumentException("Invalid weight conversion.");
        }

        public static double ConvertTemperature(double value, string fromUnit, string toUnit)
        {
            if (fromUnit == toUnit) return value;
            return fromUnit switch
            {
                "celsius" => toUnit switch { "fahrenheit" => value * 9 / 5 + 32, "kelvin" => value + 273.15, _ => throw new ArgumentException("Invalid conversion.") },
                "fahrenheit" => toUnit switch { "celsius" => (value - 32) * 5 / 9, "kelvin" => (value - 32) * 5 / 9 + 273.15, _ => throw new ArgumentException("Invalid conversion.") },
                "kelvin" => toUnit switch { "celsius" => value - 273.15, "fahrenheit" => (value - 273.15) * 9 / 5 + 32, _ => throw new ArgumentException("Invalid conversion.") },
                _ => throw new ArgumentException("Invalid temperature conversion."),
            };
        }
    }
}
