using System;
using System.Collections.Generic;

namespace MBus.Models {

    public class ListBase {
        public List<byte> RawData { get; set; }
        public DateTime Date { get; set; }
        public ListType Type { get; set; }
    }
}