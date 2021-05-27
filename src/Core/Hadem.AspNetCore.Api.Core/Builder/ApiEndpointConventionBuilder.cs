// Copyright (c) {Hadem.AspNetCore.Api}. All rights reserved.

namespace Hadem.AspNetCore.Api.Core.Builder
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Builds conventions that will be used for customization of Hub <see cref="EndpointBuilder"/> instances.
    /// </summary>
    public sealed class ApiEndpointConventionBuilder : IEndpointConventionBuilder
    {
        private readonly List<Action<EndpointBuilder>>? _conventions;
        private readonly List<IEndpointConventionBuilder>? _endpointConventionBuilders;

        internal ApiEndpointConventionBuilder(List<Action<EndpointBuilder>> conventionsActions)
        {
            this._conventions = conventionsActions;
        }

        internal ApiEndpointConventionBuilder(IEndpointConventionBuilder endpointConventionBuilder)
        {
            this._endpointConventionBuilders = new List<IEndpointConventionBuilder>() { endpointConventionBuilder };
            this._conventions = new List<Action<EndpointBuilder>>();
        }

        internal ApiEndpointConventionBuilder(List<IEndpointConventionBuilder> endpointConventionBuilders)
        {
            this._endpointConventionBuilders = endpointConventionBuilders;
            this._conventions = new List<Action<EndpointBuilder>>();
        }

        /// <summary>
        /// Adds the specified convention to the builder. Conventions are used to customize <see cref="EndpointBuilder"/> instances.
        /// </summary>
        /// <param name="convention">The convention to add to the builder.</param>
        public void Add(Action<EndpointBuilder> convention)
        {
            this._conventions!.Add(convention);
            foreach (var endpointConvention in this._endpointConventionBuilders!)
            {
                endpointConvention.Add(convention);
            }
        }
    }
}
