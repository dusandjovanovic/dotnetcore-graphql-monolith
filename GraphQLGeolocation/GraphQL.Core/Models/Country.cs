using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Core.Classes;

namespace GraphQL.Core.Models
{
    [Table("country")]
    public partial class Country : BaseEntity
    {
        public string Name { get; set; }
        public string continent { get; set; }
    }
}