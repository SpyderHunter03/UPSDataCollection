namespace UPSDataLibrary
{
    public class UPSData
    {
        public int Id { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public int BatteryCapacityPercentage { get; set; }
        public string? LastPowerEvent { get; set; }
        public int LoadWatt { get; set; }
        public int LoadPercentage { get; set; }
        public string? ModelName { get; set; }
        public int OutputVoltage { get; set; }
        public string? PowerSupplyBy { get; set; }
        public int RatingPower { get; set; }
        public int RatingVoltage { get; set; }
        public int RemainingTimeInMin { get; set; }
        public string? State { get; set; }
        public string? TestResult { get; set; }
        public int UtilityVoltage { get; set; }
    }
}
