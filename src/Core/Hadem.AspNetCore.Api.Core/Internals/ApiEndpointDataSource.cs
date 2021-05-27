// Copyright (c) {Hadem.AspNetCore.Api}. All rights reserved.

namespace Hadem.AspNetCore.Api.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Primitives;

    public class ApiEndpointDataSource : EndpointDataSource
    {
        private readonly List<DefaultEndpointConventionBuilder> _endpointConventionBuilders;

        public ApiEndpointDataSource()
        {
            this._endpointConventionBuilders = new List<DefaultEndpointConventionBuilder>();
        }

        public override IReadOnlyList<Endpoint> Endpoints => this._endpointConventionBuilders.Select(e => e.EndpointBuilder.Build()).ToArray();

        public override IChangeToken GetChangeToken() => NullChangeToken.Singleton;

        public IEndpointConventionBuilder AddEndpointBuilder(EndpointBuilder endpointBuilder)
        {
            var builder = new DefaultEndpointConventionBuilder(endpointBuilder);
            this._endpointConventionBuilders.Add(builder);
            return builder;
        }
    }
}
