using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class RatingRequest
    {
        [Required]
        [StringLength(10, ErrorMessage = "Moody's rating cannot be longer than 10 characters.")]
        public string MoodysRating { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "S&P rating cannot be longer than 10 characters.")]
        public string SandPRating { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Fitch rating cannot be longer than 10 characters.")]
        public string FitchRating { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Order number must be a positive integer.")]
        public int OrderNumber { get; set; }

        public Rating ToRating()
        {
            return new Rating
            {
                MoodysRating = MoodysRating,
                SandPRating = SandPRating,
                FitchRating = FitchRating,
                OrderNumber = OrderNumber
            };
        }
    }
}
