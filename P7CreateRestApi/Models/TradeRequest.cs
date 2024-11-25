using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class TradeRequest
    {

       
        [Required]
        [StringLength(50, ErrorMessage = "Account cannot be longer than 50 characters.")]
        public string Account { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Account type cannot be longer than 50 characters.")]
        public string AccountType { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Buy quantity must be a non-negative number.")]
        public float BuyQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Sell quantity must be a non-negative number.")]
        public float SellQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Buy price must be a non-negative number.")]
        public float BuyPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Sell price must be a non-negative number.")]
        public double SellPrice { get; set; }

        [Required]
        public DateTime TradeDate { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Trade security cannot be longer than 100 characters.")]
        public string TradeSecurity { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Trade status cannot be longer than 50 characters.")]
        public string TradeStatus { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Trader cannot be longer than 50 characters.")]
        public string Trader { get; set; }

        [StringLength(50, ErrorMessage = "Benchmark cannot be longer than 50 characters.")]
        public string Benchmark { get; set; }

        [StringLength(50, ErrorMessage = "Book cannot be longer than 50 characters.")]
        public string Book { get; set; }

        [StringLength(50, ErrorMessage = "Creation name cannot be longer than 50 characters.")]
        public string CreationName { get; set; }

        public DateTime CreationDate { get; set; }

        [StringLength(50, ErrorMessage = "Revision name cannot be longer than 50 characters.")]
        public string RevisionName { get; set; }

        public DateTime RevisionDate { get; set; }

        [StringLength(50, ErrorMessage = "Deal name cannot be longer than 50 characters.")]
        public string DealName { get; set; }

        [StringLength(50, ErrorMessage = "Deal type cannot be longer than 50 characters.")]
        public string DealType { get; set; }

        [StringLength(50, ErrorMessage = "Source list ID cannot be longer than 50 characters.")]
        public string SourceListId { get; set; }

        [StringLength(10, ErrorMessage = "Side cannot be longer than 10 characters.")]
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
