using System.Collections.Generic;

namespace GraphQL.API.Options
{
    public class CompressionOptions
    {
        public CompressionOptions()
        {
            MimeTypes = new List<string>();
        }
        
        public List<string> MimeTypes { get; }
    }
}