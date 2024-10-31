using Findexium.Domain.Models;

namespace Findexium.Api.Models
{
    public class RatingRequest
    {
        public string MoodysRating { get; set; }
        public string SandPrating { get; set; }
        public string FitchRating { get; set; }
        public int OrderNumber { get; set; }

        internal Rating ToRating()
        {
            return new Rating
            {
                MoodysRating = MoodysRating,
                SandPRating = SandPrating,
                FitchRating = FitchRating,
                OrderNumber = OrderNumber
            };  
        }
    }
}
