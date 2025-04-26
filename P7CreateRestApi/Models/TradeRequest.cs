using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class TradeRequest
    {

       
        [Required]
        [StringLength(50, ErrorMessage = "Account ne peut pas etre plus long que 50 caracteres.")]
        public string Account { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Le type de compte ne peut pas etre plus long que 50 caracteres.")]
        public string AccountType { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Buy currency ne peut pas etre plus long que 50 caracteress.")]
        public string? BuyCurrency { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Sell currency ne peut pas etre plus long que 50 caracteres.")]
        public string? SellCurrency { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Buy quantity ne peut etre un nombre négatif.")]
        public float BuyQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Sell quantity ne peut etre un nombre negatif.")]
        public float SellQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Buy price ne peut etre un nombre negatif.")]
        public float BuyPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Sell price ne peut etre un nombre negatif.")]
        public double SellPrice { get; set; }

        [Required]
        public DateTime TradeDate { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Trade security ne peut depasser 100 caracteres.")]
        public string TradeSecurity { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Trade status ne peut depasser 50 caracteres.")]
        public string TradeStatus { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Trader ne peut dépasser 50 caracteres.")]
        public string Trader { get; set; }

        [StringLength(50, ErrorMessage = "Benchmark ne peut dépasser 50 caracteres.")]
        public string Benchmark { get; set; }

        [StringLength(50, ErrorMessage = "Book ne peut dépasser 50 caracteres.")]
        public string Book { get; set; }

        [StringLength(50, ErrorMessage = "Creation name ne peut dépasser 50 caracteres.")]
        public string CreationName { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Creation date doit etre une date valide.")]
        public DateTime CreationDate { get; set; }

        [StringLength(50, ErrorMessage = "Revision name ne peut dépasser 50 caracteres.")]
        public string RevisionName { get; set; }


        public DateTime RevisionDate { get; set; }

        [StringLength(50, ErrorMessage = "Deal name ne peut dépasser 50 caracteres.")]
        public string DealName { get; set; }

        [StringLength(50, ErrorMessage = "Deal type ne peut dépasser 50 caracteres.")]
        public string DealType { get; set; }

        [StringLength(50, ErrorMessage = "Source list ID ne peut dépasser 50 caracteres.")]
        public string SourceListId { get; set; }

        [StringLength(10, ErrorMessage = "Side ne peut dépasser  caracteres.")]
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
