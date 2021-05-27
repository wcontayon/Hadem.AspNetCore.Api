// Copyright (c) {Hadem.AspNetCore.Api}. All rights reserved.

namespace Hadem.AspNetCore.Api.Core.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Hadem.AspNetCore.Api.Core.Attributes;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Routing.Patterns;

    /// <summary>
    /// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to define HTTP API endpoints.
    /// </summary>
    public static class ApiModuleEndpointConventionBuildExtensions
    {
        // Avoid creating a new array every call
        private static readonly string[] GetVerb = new[] { "GET" };
        private static readonly string[] PostVerb = new[] { "POST" };
        private static readonly string[] PutVerb = new[] { "PUT" };
        private static readonly string[] DeleteVerb = new[] { "DELETE" };

        /// <summary>
        /// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP GET requests
        /// for the specified pattern.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="api">The <typeparamref name="TApi"/> api to register with the endpoint.</param>
        /// <returns>A <see cref="IEndpointConventionBuilder"/> that can be used to further customize the endpoint.</returns>
        public static IEndpointConventionBuilder MapApi<TApi>(
            this IEndpointRouteBuilder endpoints,
            TApi api)
            where TApi : class, IApi
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }

            // Create delegate from MethodInfo with ExposeEndpointAttribute
            var endpointMethods = api.GetType().GetMethods()
                .Where(m => m.GetCustomAttributes<ExposeEndpointAttribute>().Any())
                .ToArray();

            var builders = new List<IEndpointConventionBuilder>();

            foreach (MethodInfo method in endpointMethods)
            {
                var metaData = method.GetCustomAttribute<ExposeEndpointAttribute>();
                var builder = MapMethods(
                    endpoints,
                    metaData?.Pattern!,
                    metaData?.Name ?? $"{metaData?.HttpMethod.ToString()}: {method.Name}",
                    MapHttpVerbs(metaData!.HttpMethod),
                    RequestDelegateFactory.Create(method));

                builders.Add(builder);
            }

            return new CompositeEndpointConventionBuilder(builders);
        }

        /// <summary>
        /// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests
        /// for the specified HTTP methods and pattern.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="pattern">The route pattern.</param>
        /// <param name="httpMethods">HTTP methods that the endpoint will match.</param>
        /// <param name="action">The delegate executed when the endpoint is matched.</param>
        /// <returns>A <see cref="IEndpointConventionBuilder"/> that can be used to further customize the endpoint.</returns>
        public static IEndpointConventionBuilder MapMethods(
           this IEndpointRouteBuilder endpoints,
           string pattern,
           string routeEndpointName,
           IEnumerable<string> httpMethods,
           Delegate action)
        {
            if (httpMethods is null)
            {
                throw new ArgumentNullException(nameof(httpMethods));
            }

            var builder = endpoints.Map(RoutePatternFactory.Parse(pattern), routeEndpointName, action);
            builder.WithDisplayName($"{pattern} HTTP: {string.Join(", ", httpMethods)}");
            builder.WithMetadata(new HttpMethodMetadata(httpMethods));
            return builder;
        }

        /// <summary>
        /// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests
        /// for the specified pattern.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="pattern">The route pattern.</param>
        /// <param name="action">The delegate executed when the endpoint is matched.</param>
        /// <returns>A <see cref="IEndpointConventionBuilder"/> that can be used to further customize the endpoint.</returns>
        public static ApiEndpointConventionBuilder Map(
            this IEndpointRouteBuilder endpoints,
            RoutePattern pattern,
            string routeEndpointName,
            Delegate action)
        {
            if (endpoints is null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            if (pattern is null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            const int defaultOrder = 0;

            var builder = new RouteEndpointBuilder(
                RequestDelegateFactory.Create(action),
                pattern,
                defaultOrder)
            {
                DisplayName = pattern.RawText ?? routeEndpointName,
            };

            // Add delegate attributes as metadata
            var attributes = action.Method.GetCustomAttributes();

            // This can be null if the delegate is a dynamic method or compiled from an expression tree
            if (attributes is not null)
            {
                foreach (var attribute in attributes)
                {
                    builder.Metadata.Add(attribute);
                }
            }

            var dataSource = EnsureEndpointDataSource(endpoints);

            return new ApiEndpointConventionBuilder(dataSource.AddEndpointBuilder(builder));
        }

        /// <summary>
        /// Get or create the <see cref="ApiEndpointDataSource"/>.
        /// </summary>
        /// <param name="endpoints"><see cref="IEndpointRouteBuilder"/>.</param>
        /// <returns><see cref="ApiEndpointDataSource"/>.</returns>
        private static ApiEndpointDataSource EnsureEndpointDataSource(IEndpointRouteBuilder endpoints)
        {
            var dataSource = endpoints.DataSources.OfType<ApiEndpointDataSource>().FirstOrDefault();
            if (dataSource is null)
            {
                dataSource = new ApiEndpointDataSource();
                endpoints.DataSources.Add(dataSource);
            }

            return dataSource;
        }

        private static string[] MapHttpVerbs(HttpMethod method)
            => method switch
            {
                HttpMethod.Get => GetVerb,
                HttpMethod.Patch => new[] { "PATCH" },
                HttpMethod.Post => PostVerb,
                HttpMethod.Put => PutVerb,
                HttpMethod.Delete => DeleteVerb,
                _ => GetVerb,
            };

        private class CompositeEndpointConventionBuilder : IEndpointConventionBuilder
        {
            private readonly List<IEndpointConventionBuilder> _endpointConventionBuilders;

            public CompositeEndpointConventionBuilder(List<IEndpointConventionBuilder> endpointConventionBuilders)
            {
                this._endpointConventionBuilders = endpointConventionBuilders;
            }

            public void Add(Action<EndpointBuilder> convention)
            {
                foreach (var endpointConventionBuilder in this._endpointConventionBuilders)
                {
                    endpointConventionBuilder.Add(convention);
                }
            }
        }
    }
}
