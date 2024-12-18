using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class BidRequest
    {
        [Required(ErrorMessage = "Account est requis.")]
        [Display(Name = "Compte")]
        public string Account { get; set; }

      
        [Required(ErrorMessage = "BidType est requis.")]
        [Display(Name = "Type d'offre")]
        public string BidType { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "BidQuantity doit etre un nombre positif .")]
        [Display(Name = "Quantité d'offre")]
        public double? BidQuantity { get; set; }

      

     

        public BidList ToBid()
        {
            return new BidList
            {
                Account = Account,
                BidType = BidType,
                BidQuantity = BidQuantity,           
               
             
            };
        }
    }
}
