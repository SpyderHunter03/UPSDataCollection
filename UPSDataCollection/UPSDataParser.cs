using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using UPSDataLibrary;

namespace UPSDataCollection
{
    public static partial class UPSDataParser
    {
        public static UPSData CreateFromJsonObject(JsonObject? data)
        {
            return new UPSData
            {
                BatteryCapacityPercentage = ParsePercentage(data?["Battery Capacity"]?.GetValue<string>() ?? ""),
                LastPowerEvent = data?["Last Power Event"]?.GetValue<string>(),
                LoadWatt = ParseWattage(data?["Load"]?.GetValue<string>() ?? ""),
                LoadPercentage = ParsePercentage(data?["Load"]?.GetValue<string>() ?? ""),
                ModelName = data?["Model Name"]?.GetValue<string>(),
                OutputVoltage = ParseVoltage(data?["Output Voltage"]?.GetValue<string>() ?? ""),
                PowerSupplyBy = data?["Power Supply by"]?.GetValue<string>(),
                RatingPower = ParseWattage(data?["Rating Power"]?.GetValue<string>() ?? ""),
                RatingVoltage = ParseVoltage(data?["Rating Voltage"]?.GetValue<string>() ?? ""),
                RemainingTimeInMin = ParseTime(data?["Remaining Runtime"]?.GetValue<string>() ?? ""),
                State = data?["State"]?.GetValue<string>(),
                TestResult = data?["Test Result"]?.GetValue<string>(),
                UtilityVoltage = ParseVoltage(data?["Utility Voltage"]?.GetValue<string>() ?? "")
            };
        }

        private static int ParsePercentage(string load) =>
            ParseWithRegex(load, PercentageRegex());

        private static int ParseVoltage(string load) =>
            ParseWithRegex(load, VoltageRegex());

        private static int ParseWattage(string load) =>
            ParseWithRegex(load, WattRegex());

        private static int ParseTime(string load) =>
            ParseWithRegex(load, RemainingTimeRegex());

        private static int ParseWithRegex(string load, Regex regex)
        {
            var match = regex.Match(load);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            return default;
        }

        [GeneratedRegex(@"(\d+) %")]
        private static partial Regex PercentageRegex();

        [GeneratedRegex(@"(\d+) V")]
        private static partial Regex VoltageRegex();

        [GeneratedRegex(@"(\d+) Watt")]
        private static partial Regex WattRegex();

        [GeneratedRegex(@"(\d+) min")]
        private static partial Regex RemainingTimeRegex();
    }
}
