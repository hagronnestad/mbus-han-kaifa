namespace MBus.Models {

    public class List3 : List2 {
        public uint CumulativeActiveImportEnergy { get; set; }
        public uint CumulativeActiveExportEnergy { get; set; }
        public uint CumulativeReactiveImportEnergy { get; set; }
        public uint CumulativeReactiveExportEnergy { get; set; }

        public double CumulativeActiveImportEnergyDouble => CumulativeActiveImportEnergy / 1000.0f;
        public double CumulativeActiveExportEnergyDouble => CumulativeActiveExportEnergy / 1000.0f;
        public double CumulativeReactiveImportEnergyDouble => CumulativeReactiveImportEnergy / 1000.0f;
        public double CumulativeReactiveExportEnergyDouble => CumulativeReactiveExportEnergy / 1000.0f;
    }
}