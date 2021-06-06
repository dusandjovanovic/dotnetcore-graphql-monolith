using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.API.Graph.Schema;
using GraphQL.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraphQL.API.Controllers
{
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly GraphQLSchema _schema;
        private readonly IServiceProvider _provider;

        public GraphQLController(IDocumentExecuter documentExecuter, GraphQLSchema schema, IServiceProvider provider)
        {
            _provider = provider;
            _documentExecuter = documentExecuter;
            _schema = schema;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            var inputs = query.Variables?.ToString().ToInputs();

            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                OperationName = query.OperationName,
                Inputs = inputs               
            };
           

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (!(result.Errors?.Count > 0)) return Ok(result);
            var graphQLErrors = new List<string>();
            var errors = result.Errors.GetEnumerator();
            while (errors.MoveNext())
            {
                graphQLErrors.Add(errors.Current.InnerException != null ? errors.Current.InnerException.Message : errors.Current.Message);
            }

            return BadRequest(new { result, graphQLErrors });
        }

    }
}