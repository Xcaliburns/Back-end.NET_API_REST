namespace Findexium.Api.Models
{
    public class CurvePointResponse
    {
        public int Id { get; set; }
        public int? CurveId { get; set; }       
        public double? Term { get; set; }
        public double? CurvePointValue { get; set; }
       

    }
}
