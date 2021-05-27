// Copyright (c) {Hadem.AspNetCore.Api}. All rights reserved.

namespace Hadem.AspNetCore.Api.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Interface used to configure all <see cref="IApi"/> of the project.
    /// Only one <see cref="IApiModule"/> by project.
    /// </summary>
    internal interface IApiModule
    {
        /// <summary>
        /// Gets all Api to configure with their endpoints.
        /// </summary>
        IEnumerable<IApi> APIs { get; }

        /// <summary>
        /// Gets all endpoints and the <see cref="RequestDelegate"/> associate.
        /// </summary>
        IDictionary<(string Method, string RoutePattern), RequestDelegate> Routes { get; }

        /// <summary>
        /// Gets a value indicating whether all api endpoint should be executed
        /// with authentication.
        /// </summary>
        bool RequireAuth { get; }

        /// <summary>
        /// Add a <see cref="IApi"/> to the <see cref="IApiModule"/>.
        /// </summary>
        /// <param name="api">The <see cref="IApi"/> to add.</param>
        /// <returns>The current <see cref="IApiModule"/>.</returns>
        IApiModule AddApi(IApi api);
    }
}
