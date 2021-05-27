// Copyright (c) {Hadem.AspNetCore.Api}. All rights reserved.

namespace Hadem.AspNetCore.Api.Core.Attributes
{
    using System;

    /// <summary>
    /// Define an endpoint to be exposed by <see cref="Api.Core.Builder.ApiEndpointConventionBuilder"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ExposeEndpointAttribute : Attribute
    {
        public ExposeEndpointAttribute(
            string name = "",
            HttpMethod httpMethod = HttpMethod.Get,
            string pattern = "")
        {
            this.HttpMethod = httpMethod;
            this.Name = name;
            this.Pattern = pattern;
        }

        public ExposeEndpointAttribute(string pattern) => this.Pattern = pattern;

        /// <summary>
        /// Gets the name of the Endpoint.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Gets the routing pattern to use.
        /// </summary>
        public string? Pattern { get; }

        /// <summary>
        /// Gets the <see cref="HttpMethod"/> of the endpoint to be exposed.
        /// </summary>
        public HttpMethod HttpMethod { get; }
    }

    public enum HttpMethod
    {
        Get,
        Post,
        Put,
        Delete,
        Patch,
    }
}
