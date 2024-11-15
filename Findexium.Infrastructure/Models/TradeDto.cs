using Findexium.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Infrastructure.Models
{
    public class TradeDto
    {
        public int TradeId { get; set; }
        public string Account { get; set; }
        public string AccountType { get; set; }
        public double? BuyQuantity { get; set; }
        public double? SellQuantity { get; set; }
        public double? BuyPrice { get; set; }
        public double? SellPrice { get; set; }
        public DateTime? TradeDate { get; set; }
        public string TradeSecurity { get; set; }
        public string TradeStatus { get; set; }
        public string Trader { get; set; }
        public string Benchmark { get; set; }
        public string Book { get; set; }
        public string CreationName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string RevisionName { get; set; }
        public DateTime? RevisionDate { get; set; }
        public string DealName { get; set; }


        public TradeDto( string account, string accountType, double? buyQuantity, double? sellQuantity, double? buyPrice, double? sellPrice, DateTime? tradeDate, string tradeSecurity, string tradeStatus, string trader, string benchmark, string book, string creationName, DateTime? creationDate, string revisionName, DateTime? revisionDate, string dealName)
        {
           
            Account = account;
            AccountType = accountType;
            BuyQuantity = buyQuantity;
            SellQuantity = sellQuantity;
            BuyPrice = buyPrice;
            SellPrice = sellPrice;
            TradeDate = tradeDate;
            TradeSecurity = tradeSecurity;
            TradeStatus = tradeStatus;
            Trader = trader;
            Benchmark = benchmark;
            Book = book;
            CreationName = creationName;
            CreationDate = creationDate;
            RevisionName = revisionName;
            RevisionDate = revisionDate;
            DealName = dealName;
        }

        internal Trade ToTrade()
        {
            return new Trade
            {
                TradeId=TradeId,
                Account = Account,
                AccountType = AccountType,
                BuyQuantity = BuyQuantity,
                SellQuantity = SellQuantity,
                BuyPrice = BuyPrice,
                SellPrice = SellPrice,
                TradeDate = TradeDate,
                TradeSecurity = TradeSecurity,
                TradeStatus = TradeStatus,
                Trader = Trader,
                Benchmark = Benchmark,
                Book = Book,
                CreationName = CreationName,
                CreationDate = CreationDate,
                RevisionName = RevisionName,
                RevisionDate = RevisionDate,
                DealName = DealName,

            };


        }


    }       
}
