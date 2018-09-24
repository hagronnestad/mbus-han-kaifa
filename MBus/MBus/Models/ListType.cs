namespace MBus.Models {

    public enum ListType {
        List1 = 1,    // Active Power
        List9 = 9,    // List2, but for single phase meters?
        List2 = 13,   // 3 phase meters?
        List3 = 18    // List2 + cumulative data (meter reading)?
    }
}