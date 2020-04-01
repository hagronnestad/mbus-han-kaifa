namespace MBus.Models {

    public enum ListType {
        List1 = 0x01,               // List1: Active Power (All meters)
        List2SinglePhase = 0x09,    // List2: (Single phase meters)
        List2 = 0x0D,               // List2: (3 phase meters)
        List3 = 0x12,               // List3: List2 + cumulative data (meter reading)
        List3SinglePhase = 0x0E     // List3: List2 + cumulative data (meter reading) (Single phase meters)
    }
}