using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Core.Classes;

namespace GraphQL.Core.Models
{
    [Table("city")]
    public partial class City : BaseEntity
    {
        public string Name { get; set; }
        public int? Population { get; set; }
        public int CountryId { get; set; }
        
        [ForeignKey("CountryId")]
        public Country Country { get; set; }
    }
}