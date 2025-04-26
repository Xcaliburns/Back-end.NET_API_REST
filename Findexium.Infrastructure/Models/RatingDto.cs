using Findexium.Domain.Models;

namespace Findexium.Infrastructure.Models
{
    public class RatingDto
    {
        public int Id { get;  set; }
        public string MoodysRating { get; set; }
        public string SandPRating { get; set; }
        public string FitchRating { get; set; }
        public int OrderNumber { get; set; }
       

        public RatingDto(int id,string moodysRating, string sandPRating, string fitchRating, int orderNumber)
        {
            Id = id;
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
