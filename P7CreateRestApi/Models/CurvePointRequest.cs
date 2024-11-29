using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class CurvePointRequest
    {
        [Required(ErrorMessage = "CurveId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CurveId must be a positive number.")]
        public int CurveId { get; set; }

      

        [Range(0, double.MaxValue, ErrorMessage = "Term must be a positive number.")]
        public double Term { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "CurvePointValue must be a positive number.")]
        public double CurvePointValue { get; set; }

       

        internal CurvePoint ToCurvePoint()
        {
            return new CurvePoint
            {
                CurveId = CurveId,              
                Term = Term,
                CurvePointValue = CurvePointValue,
                //test
                AsOfDate = DateTime.Now,
                CreationDate = DateTime.Now

            };
        }
    }

}
