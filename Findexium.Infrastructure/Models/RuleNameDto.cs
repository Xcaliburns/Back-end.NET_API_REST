using Findexium.Domain.Models;

namespace Findexium.Infrastructure.Models
{
    public class RuleNameDto
    {
        public int Id { get;  set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Json { get; set; }
        public string  Template { get; set; }
        public string SqlStr { get; set; }
        public string SqlPart { get; set; }

        public RuleNameDto(int id,string name, string description, string json, string template,string sqlStr, string sqlPart )
        {
            Id = id;
            Name = name;
            Description = description;
            Json = json;
            Template = template;
            SqlStr = sqlStr;
            SqlPart = sqlPart;
        }

        public RuleName ToRuleName()
        {
            return new RuleName
            {
                Id = Id,
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
