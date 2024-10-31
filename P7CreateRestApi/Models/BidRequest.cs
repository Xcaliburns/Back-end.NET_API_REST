using Findexium.Domain.Models;

namespace Findexium.Api.Models
{
    public class BidRequest
    {
        public string Account { get; set; }
        public string BidType { get; set; }
        public double? BidQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public double Bid { get; set; }
        public double? Ask { get; set; }
        public string Benchmark { get; set; }
        public DateTime BidListDate { get; set; }
        public string Commentary { get; set; }

        public string BidSecurity { get; set; }
        public string BidStatus { get; set; }
        public string Trader { get; set; }
        public string Book { get; set; }
        public string CreationName { get; set; }
        public DateTime CreationDate { get; set; }
        public string RevisionName { get; set; }
        public DateTime RevisionDate { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public string SourceListId { get; set; }
        public string Side { get; set; }
        public string Traffic { get; set; }
       
        internal BidList ToBid()
        {
            return new BidList
            {
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
                Side = Side,
               
            };
        }

    }
}
