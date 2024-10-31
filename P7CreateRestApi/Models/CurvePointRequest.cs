using Findexium.Domain.Models;

namespace Findexium.Api.Models
{
    public class CurvePointRequest
    {
        public int CurveId { get; set; }
        public DateTime AsOfDate { get; set; }
        public double Term { get; set; }
        public double CurvePointValue { get; set; }
        public DateTime CreationDate { get; set; }
    

    internal  CurvePoint ToCurvePoint()
    {
        return new CurvePoint
        {
            CurveId = CurveId,
            AsOfDate = AsOfDate,
            Term = Term,
            CurvePointValue = CurvePointValue,
            CreationDate = CreationDate
        };
    }
    }

}
