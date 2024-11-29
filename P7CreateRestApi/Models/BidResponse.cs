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
        public string Account{get;set; }
        public string BidType { get; set; }
        public double? BidQuantity { get; set; }
      
    }
}
