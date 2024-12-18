using Findexium.Domain.Models;

namespace Findexium.Infrastructure.Models
{
    public class CurvePointsDto
    {
        public int Id { get;  set; }
        public int CurveId { get; set; }
        public DateTime AsOfDate { get; set; }
        public double Term { get; set; }
        public double CurvePointValue { get; set; }
        public DateTime CreationDate { get; set; }

        public CurvePointsDto(int curveId, DateTime asOfDate, double term, double curvePointValue, DateTime creationDate)
        {
            CurveId = curveId;
            AsOfDate = asOfDate;
            Term = term;
            CurvePointValue = curvePointValue;
            CreationDate = creationDate;
        }

        internal CurvePoint ToCurvePoint()
        {
            return new CurvePoint
            {
                Id=Id,
                CurveId = CurveId,
                AsOfDate = AsOfDate,
                Term = Term,
                CurvePointValue = CurvePointValue,
                CreationDate = CreationDate
            };
        }
    }
}
