using GraphQL.API.Helpers;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Models
{
    public class StatsMutation : ObjectGraphType
    {
        public StatsMutation(ContextServiceLocator contextServiceLocator)
        {
            Name = "CreatePlayerMutation";

            Field<PlayerType>(
                "createPlayer",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<PlayerInputType>> { Name = "player"}),
                resolve: context =>
                {
                    var player = context.GetArgument<Player>("player");
                    return contextServiceLocator.PlayerRepository.Add(player);
                }
            );
        }
    }
}