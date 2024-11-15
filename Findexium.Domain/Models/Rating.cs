
namespace Findexium.Domain.Models // namespace Dot.Net.WebApi.Controllers.Domain Correction effectuée
{
    public class Rating
    {
        

        public int Id { get; set; }
        public string MoodysRating { get; set; }
        public string SandPRating { get; set; }
        public string FitchRating { get; set; }
        public int OrderNumber { get; set; }

      
    }
}