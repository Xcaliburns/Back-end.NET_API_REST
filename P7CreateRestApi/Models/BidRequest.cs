using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class BidRequest
    {
        [Required(ErrorMessage = "Account is required.")]
        public string Account { get; set; }
    
        public string BidType { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "BidQuantity must be a positive number.")]
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
