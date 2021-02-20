using GraphQL.API.Types;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Data.Services;
using GraphQL.Types;

namespace GraphQL.API.Schemas
{
    /**
     * mutation createHuman($human: HumanInput!) {
     *   createHuman(human: $human)
     *   {
     *     id
     *     name
     *     dateOfBirth
     *     appearsIn
     *   }
     * }
     *
     * {
     *  "human": {
     *     "name": "Dushan Jovanovic",
     *     "homePlanet": "Earth",
     *     "dateOfBirth": "2000-01-01",
     *     "appearsIn": [ "NEWHOPE" ]
     *   }
     * }
     */
    public class MutationObject : ObjectGraphType<object>
    {
        public MutationObject(IClockService clockService, IHumanRepository humanRepository)
        {
            Name = "Mutation";
            Description = "The mutation type, represents all updates we can make to our data.";

            FieldAsync<HumanObject, Human>(
                "createHuman",
                "Create a new human.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<HumanInputObject>>()
                    {
                        Name = "human",
                        Description = "The human you want to create.",
                    }),
                resolve: context =>
                {
                    var human = context.GetArgument<Human>("human");
                    var now = clockService.UtcNow;
                    human.Created = now;
                    human.Modified = now;
                    return humanRepository.AddHumanAsync(human, context.CancellationToken);
                });
        }
    }
}