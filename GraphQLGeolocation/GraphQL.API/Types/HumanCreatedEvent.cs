using GraphQL.Core.Data;

namespace GraphQL.API.Types
{
    public class HumanCreatedEvent : HumanObject
    {
        public HumanCreatedEvent(IHumanRepository humanRepository) 
            : base(humanRepository) { }
    }
}