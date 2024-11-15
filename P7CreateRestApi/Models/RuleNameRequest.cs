using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;
namespace Findexium.Api.Models
{
    public class RuleNameRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Required]
        public string Json { get; set; }

        [Required]
        public string Template { get; set; }

        [Required]
        public string SqlStr { get; set; }

        [Required]
        public string SqlPart { get; set; }

        internal RuleName ToRuleName()
        {
            return new RuleName
            {
                Name = Name,
                Description = Description,
                Json = Json,
                Template = Template,
                SqlStr = SqlStr,
                SqlPart = SqlPart
            };
        }
    }
}
