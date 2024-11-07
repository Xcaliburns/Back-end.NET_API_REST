using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Findexium.Api.Models
{
    public class BidListModel
    {
        
        public int BidListId;
        public string Account;
        public string BidType;
        public double? BidQuantity;
        public double? AskQuantity;
        public double Bid;
        public double? Ask;
        public string Benchmark;
        public DateTime? BidListDate;
        public string Commentary;
        public string BidSecurity;
        public string BidStatus;
        public string Trader;
        public string Book;
        public string CreationName;
        public DateTime? CreationDate;
        public string RevisionName;
        public DateTime? RevisionDate;
        public string DealName;
        public string DealType;
        public string SourceListId;
        public string Side;
    }
}
