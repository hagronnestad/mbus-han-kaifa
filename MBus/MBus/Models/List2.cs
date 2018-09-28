namespace MBus.Models {

    public class List2 : ListBase {
        public string ListVersionIdentifier { get; set; }
        public string MeterId { get; set; }
        public string MeterType { get; set; }

        public uint ActivePowerImport { get; set; }
        public uint ActivePowerExport { get; set; }
        public uint ReactivePowerImport { get; set; }
        public uint ReactivePowerExport { get; set; }
        public int CurrentL1 { get; set; }
        public int CurrentL2 { get; set; }
        public int CurrentL3 { get; set; }
        public uint VoltageL1 { get; set; }
        public uint VoltageL2 { get; set; }
        public uint VoltageL3 { get; set; }

        public double CurrentL1Double => CurrentL1 / 1000.0f;
        public double VoltageL1Double => VoltageL1 / 10.0f;
    }
}