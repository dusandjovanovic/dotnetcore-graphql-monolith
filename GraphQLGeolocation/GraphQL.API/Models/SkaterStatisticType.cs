using GraphQL.Core.Models;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GraphQL.API.Models
{
    public class SkaterStatisticType : ObjectGraphType<SkaterStatistic>
    {
        public SkaterStatisticType()
        {
            Field(x => x.Id);
            Field(x => x.SeasonId);
            Field(x => x.Season.Name);
            Field(x => x.League.Abbreviation).Name("league");
            Field(x => x.Team.Name).Name("team");
            Field<IntGraphType>("gp", resolve: context => context.Source.GamesPlayed);
            Field<IntGraphType>("goals", resolve: context => context.Source.Goals);
            Field<IntGraphType>("assists", resolve: context => context.Source.Assists);
            Field<IntGraphType>("points", resolve: context => context.Source.Points);
            Field<IntGraphType>("plusMinus", resolve: context => context.Source.PlusMinus);
        }
    }
}