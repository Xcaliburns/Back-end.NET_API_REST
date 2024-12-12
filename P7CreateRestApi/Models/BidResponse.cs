using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class BidResponse
    {
        public int BidListId { get; set; }

        [Required]
        public string Account { get; set; } = string.Empty;

        [Required]
        public string BidType { get; set; } = string.Empty;

        public double? BidQuantity { get; set; }
    }
}
