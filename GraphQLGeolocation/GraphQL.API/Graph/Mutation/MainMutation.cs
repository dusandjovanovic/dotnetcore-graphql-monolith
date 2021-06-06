using System;
using GraphQL.API.Interfaces;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Mutation
{
    public class MainMutation : ObjectGraphType
    {
        public MainMutation(IServiceProvider provider, IWebHostEnvironment environment, IFieldService fieldService)
        {
            Name = "MainMutation";
            fieldService.ActivateFields(this, FieldServiceType.Mutation, environment, provider);
        }
    }
}