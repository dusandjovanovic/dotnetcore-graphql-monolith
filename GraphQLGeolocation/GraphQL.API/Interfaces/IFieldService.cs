using System;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Interfaces
{
    public interface IFieldService
    {
        void ActivateFields(ObjectGraphType objectGraphType, FieldServiceType fieldServiceType,
            IWebHostEnvironment environment, IServiceProvider provider);

        void RegisterFields();
    }

    public enum FieldServiceType
    {
        Query,
        Mutation,
        Subscription
    }
}