using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaEthereum.Models
{
    [Table("tbl_market_time")]
    public class MarketTime
    {
        public int Id { get; set; }

        public DateTime StartMarketTime { get; set; }

        public DateTime CloseMarketTime { get; set; }

        public string Remarks { get; set; }
    }
}