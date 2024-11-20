using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class BidRequest
    {
        [Required(ErrorMessage = "Account is required.")]
        public string Account { get; set; }

        [Required(ErrorMessage = "BidType is required.")]
        public string BidType { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "BidQuantity must be a positive number.")]
        public double? BidQuantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "AskQuantity must be a positive number.")]
        public double? AskQuantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Bid must be a positive number.")]
        public double Bid { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Ask must be a positive number.")]
        public double? Ask { get; set; }

        [Required(ErrorMessage = "Benchmark is required.")]
        public string Benchmark { get; set; }

        [Required(ErrorMessage = "BidListDate is required.")]
        public DateTime BidListDate { get; set; }

        public string Commentary { get; set; }

        [Required(ErrorMessage = "BidSecurity is required.")]
        public string BidSecurity { get; set; }

        [Required(ErrorMessage = "BidStatus is required.")]
        public string BidStatus { get; set; }

        [Required(ErrorMessage = "Trader is required.")]
        public string Trader { get; set; }

        [Required(ErrorMessage = "Book is required.")]
        public string Book { get; set; }

        [Required(ErrorMessage = "CreationName is required.")]
        public string CreationName { get; set; }

        [Required(ErrorMessage = "CreationDate is required.")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "RevisionName is required.")]
        public string RevisionName { get; set; }

        [Required(ErrorMessage = "RevisionDate is required.")]
        public DateTime RevisionDate { get; set; }

        [Required(ErrorMessage = "DealName is required.")]
        public string DealName { get; set; }

        [Required(ErrorMessage = "DealType is required.")]
        public string DealType { get; set; }

        [Required(ErrorMessage = "SourceListId is required.")]
        public string SourceListId { get; set; }

        [Required(ErrorMessage = "Side is required.")]
        public string Side { get; set; }

    //    [Required(ErrorMessage = "Traffic is required.")]
      //  public string Traffic { get; set; }

        public BidList ToBid()
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
