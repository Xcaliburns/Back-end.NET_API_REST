using Findexium.Domain.Models;
namespace Findexium.Api.Models
{
    public class RuleNameRequest
    {
        public string Name{ get; set; }
        public string Description { get; set; }
        public string Json { get; set; }
        public string Template { get; set; }
        public string SqlStr { get; set; }
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
