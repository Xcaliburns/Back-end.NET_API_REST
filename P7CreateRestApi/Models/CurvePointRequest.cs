using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class CurvePointRequest
    {
        [Required(ErrorMessage = "CurveId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "l'identifiant de la courbe ne peut etre negatif.")]
        [Display(Name = "Identifiant de la courbe")]
        public int CurveId { get; set; }
      
        [Range(0, double.MaxValue, ErrorMessage = "Delai ne peut etre négatif.")]
        [Display(Name = "Delai")]
        public double? Term { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "la valeur ne peut etre negative.")]
        [Display(Name = "Valeur ")]
        public double? CurvePointValue { get; set; }

       

        internal CurvePoint ToCurvePoint()
        {
            return new CurvePoint
            {
                CurveId = CurveId,              
                Term = Term ?? 0 ,
                CurvePointValue = CurvePointValue ?? 0,
                CreationDate = DateTime.Now

            };
        }
    }

}
