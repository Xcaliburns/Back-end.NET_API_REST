using Findexium.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Infrastructure.Models
{
    public class BidDto
    {
        public int BidListId { get; set; } // Define this as the primary key
        public string Account { get; set; }
        public string BidType { get; set; }
        public double? BidQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public double Bid { get; set; }
        public double? Ask { get; set; }
        public string Benchmark { get; set; }
        public DateTime? BidListDate { get; set; }
        public string Commentary { get; set; }
        public string BidSecurity { get; set; }
        public string BidStatus { get; set; }
        public string Trader { get; set; }
        public string Book { get; set; }
        public string CreationName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string RevisionName { get; set; }
        public DateTime? RevisionDate { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public string SourceListId { get; set; }
        public string Side { get; set; }

        public BidDto(int bidListId, string account, string bidType, double? bidQuantity, double? askQuantity, double bid, double? ask, string benchmark, DateTime? bidListDate, string commentary, string bidSecurity, string bidStatus, string trader, string book, string creationName, DateTime? creationDate, string revisionName, DateTime? revisionDate, string dealName, string dealType, string sourceListId, string side)
        {
            BidListId = bidListId;
            Account = account;
            BidType = bidType;
            BidQuantity = bidQuantity;
            AskQuantity = askQuantity;
            Bid = bid;
            Ask = ask;
            Benchmark = benchmark;
            BidListDate = bidListDate;
            Commentary = commentary;
            BidSecurity = bidSecurity;
            BidStatus = bidStatus;
            Trader = trader;
            Book = book;
            CreationName = creationName;
            CreationDate = creationDate;
            RevisionName = revisionName;
            RevisionDate = revisionDate;
            DealName = dealName;
            DealType = dealType;
            SourceListId = sourceListId;
            Side = side;
        }

        internal BidList ToBidList()
        {
            return new BidList
            {
                BidListId = BidListId,
                Account = Account,
                BidType = BidType,
                BidQuantity = BidQuantity,
                AskQuantity = AskQuantity,
                Bid = Bid,
                Ask = Ask,
                Benchmark = Benchmark,
                BidListDate = BidListDate,
                Commentary = Commentary,
                BidSecurity = BidSecurity,
                BidStatus = BidStatus,
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
