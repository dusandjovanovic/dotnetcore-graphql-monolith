using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.API.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace GraphQL.API.Controllers
{
    [Route("[controller]")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            var inputs = new Inputs(query.Variables.ToObject<Dictionary<string, object>>());
            var executionOptions = new ExecutionOptions()
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0) return BadRequest(result);
            return Ok(result);
        }
    }
}