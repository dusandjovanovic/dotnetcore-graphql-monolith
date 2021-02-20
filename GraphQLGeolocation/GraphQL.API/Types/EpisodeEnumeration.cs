using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types
{
    public class EpisodeEnumeration : EnumerationGraphType<Episode>
    {
        public EpisodeEnumeration()
        {
            Name = "Episode";
            Description = "One of the films";
            AddValue("NEWHOPE", "Released in 1997", 4);
            AddValue("EMPIRE", "Released in 1980", 5);
            AddValue("JEDI", "Released in 1983", 6);
        }
    }
}