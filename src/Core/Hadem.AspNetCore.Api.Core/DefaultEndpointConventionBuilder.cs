// Copyright (c) {Hadem.AspNetCore.Api}. All rights reserved.

// LICENSING NOTE: This file is from the dotnet aspnetcore repository.
// See https://github.com/dotnet/aspnetcore/blob/main/src/Http/Routing/src/DefaultEndpointConventionBuilder.cs
namespace Hadem.AspNetCore.Api.Core
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;

    internal class DefaultEndpointConventionBuilder : IEndpointConventionBuilder
    {
        private readonly List<Action<EndpointBuilder>> _conventions;

        public DefaultEndpointConventionBuilder(EndpointBuilder endpointBuilder)
        {
            this.EndpointBuilder = endpointBuilder;
            this._conventions = new List<Action<EndpointBuilder>>();
        }

        internal EndpointBuilder EndpointBuilder { get; }

        public void Add(Action<EndpointBuilder> convention)
        {
            this._conventions.Add(convention);
        }

        public Endpoint Build()
        {
            foreach (var convention in this._conventions)
            {
                convention(this.EndpointBuilder);
            }

            return this.EndpointBuilder.Build();
        }
    }
}
