using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RFIDP2P3_API.Models
{
    [DataContract]
    public class Andon
    {
        [DataMember(Name = "No")]
        public int No { get; set; }

        [DataMember(Name = "PartNumber")]
        public int PartNumber { get; set; }

        [DataMember(Name = "PartDesc")]
        public int PartDesc { get; set; }

        [DataMember(Name = "PartName")]
        public int PartName { get; set; }

        [DataMember(Name = "PrcBlank")]
        public int PrcBlank { get; set; }

        [DataMember(Name = "PrcVaccum")]
        public int PrcVaccum { get; set; }

        [DataMember(Name = "PrcOut")]
        public int PrcOut { get; set; }

        [DataMember(Name = "StockBlank")]
        public int StockBlank { get; set; }

        [DataMember(Name = "StockVaccum")]
        public int StockVaccum { get; set; }

        [DataMember(Name = "StockEnd")]
        public int StockEnd { get; set; }

        [DataMember(Name = "PrcStatus")]
        public string PrcStatus { get; set; }

        [DataMember(Name = "StockEstCasting")]
        public int StockEstCasting { get; set; }

        [DataMember(Name = "StockEstEngine")]
        public int StockEstEngine { get; set; }

        [DataMember(Name = "StockPalletP3Unit")]
        public int StockPalletP3Unit { get; set; }

        [DataMember(Name = "StockPalletP3Day")]
        public int StockPalletP3Day { get; set; }

        [DataMember(Name = "StockPalletP2Unit")]
        public int StockPalletP2Unit { get; set; }

        [DataMember(Name = "StockPalletP2Day")]
        public int StockPalletP2Day { get; set; }
    }
}
