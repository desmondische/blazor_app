using System.ComponentModel.DataAnnotations;

namespace intelometry_app.Models
{
    public class MarketDataModel
    {
        [Key]
        public int Id { get; set; }
        public int PriceHubId { get; set; }
        public string PriceHub { get; set; } = string.Empty;
        public DateTime TradeDate { get; set; }
        public DateTime DeliveryStartDate { get; set; }
        public DateTime DeliveryEndDate { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double AvgPrice { get; set; }
        public double Change { get; set; }
        public int Volume { get; set; }
    }
}
