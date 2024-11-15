using Findexium.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Infrastructure.Models
{
    public class RatingDto
    {
        public int Id { get; internal set; }
        public string MoodysRating { get; set; }
        public string SandPRating { get; set; }
        public string FitchRating { get; set; }
        public int OrderNumber { get; set; }
       

        public RatingDto(string moodysRating, string sandPRating, string fitchRating, int orderNumber)
        {
            MoodysRating = moodysRating;
            SandPRating = sandPRating;
            FitchRating = fitchRating;
            OrderNumber = orderNumber;
        }
        

        
        internal Rating ToRating()
        {
            return new Rating
            {
                Id=Id,
                MoodysRating = MoodysRating,
                SandPRating = SandPRating,
                FitchRating = FitchRating,
                OrderNumber = OrderNumber
            };
        }
    }
}
