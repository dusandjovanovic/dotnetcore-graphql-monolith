using System;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Interfaces
{
    public interface IFieldServiceItem
    {
        void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp);
    }

    public interface IFieldMutationServiceItem : IFieldServiceItem
    {
    }

    public interface IFieldQueryServiceItem : IFieldServiceItem
    {
    }
    public interface IFieldSubscriptionServiceItem : IFieldServiceItem
    {

    }
}