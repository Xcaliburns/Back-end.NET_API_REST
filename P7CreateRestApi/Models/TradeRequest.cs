using Findexium.Domain.Models;

namespace Findexium.Api.Models
{
    public class TradeRequest
    {
        public string Account { get; set; }
        public string AccountType { get; set; }
        public float BuyQuantity { get; set; }
        public float SellQuantity { get; set; }
        public float BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public DateTime TradeDate { get; set; }
        public string TradeSecurity { get; set; }
        public string TradeStatus { get; set; }
        public string Trader { get; set; }
        public string Benchmark { get; set; }
        public string Book { get; set; }
        public string CreationName { get; set; }
        public DateTime CreationDate { get; set; }
        public string RevisionName { get; set; }
        public DateTime RevisionDate { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public string SourceListId { get; set; }
        public string Side { get; set; }

        internal Trade ToTrade()
        {
            return new Trade
            {
                Account = Account,
                AccountType = AccountType,
                BuyQuantity = BuyQuantity,
                SellQuantity = SellQuantity,
                BuyPrice = BuyPrice,
                SellPrice = SellPrice,
                Benchmark = Benchmark,
                TradeDate = TradeDate,
                TradeSecurity = TradeSecurity,
                TradeStatus = TradeStatus,
                Trader = Trader,
                Book = Book,
                CreationName = CreationName,
                CreationDate = CreationDate,
                RevisionName = RevisionName,
                RevisionDate = RevisionDate,
                DealName = DealName,
                DealType = DealType,
                SourceListId = SourceListId,
                Side = Side
            };
        }
    }
}
