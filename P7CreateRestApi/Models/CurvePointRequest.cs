using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class CurvePointRequest
    {
        [Required(ErrorMessage = "CurveId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CurveId must be a positive number.")]
        public int CurveId { get; set; }

        [Required(ErrorMessage = "AsOfDate is required.")]
       // [DataType(DataType.Date)]
        public DateTime AsOfDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Term must be a positive number.")]
        public double Term { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "CurvePointValue must be a positive number.")]
        public double CurvePointValue { get; set; }

        [Required(ErrorMessage = "CreationDate is required.")]
        public DateTime CreationDate { get; set; }

        internal CurvePoint ToCurvePoint()
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
