using System;

namespace Dot.Net.WebApi.Domain
{
    public class Trade
    {

        public int TradeId;
        public string Account;
        public string AccountType;
        double? BuyQuantity;
        double? SellQuantity;
        double? BuyPrice;
        double? SellPrice;
        DateTime? TradeDate;
        public string TradeSecurity;
        public string TradeStatus;
        public string Trader;
        public string Benchmark;
        public string Book;
        public string CreationName;
        DateTime? CreationDate;
        public string RevisionName;
        DateTime? RevisionDate;
        public string DealName;
        public string DealType;
        public string SourceListId;
        public string Side;
    }
}