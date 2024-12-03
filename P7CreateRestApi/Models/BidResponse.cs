using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

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
