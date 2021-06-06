using System;

namespace GraphQL.Core.Data
{
    public interface IEntity
    {
        int Id { get; set; }
        
        DateTime? CreationDate { get; set; }
    }
}