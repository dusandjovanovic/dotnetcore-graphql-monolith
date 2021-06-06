using System;
using GraphQL.Core.Data;

namespace GraphQL.Core.Classes
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }
        
        public DateTime? CreationDate { get; set; }
    }
}