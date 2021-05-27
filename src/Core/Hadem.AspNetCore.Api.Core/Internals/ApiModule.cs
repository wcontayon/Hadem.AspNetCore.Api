// Copyright (c) {Hadem.AspNetCore.Api}. All rights reserved.

namespace Hadem.AspNetCore.Api.Core
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;

    internal class ApiModule : IApiModule
    {
        private List<IApi>? _apis;
        private bool _requireAuth;
        private IDictionary<(string Method, string RoutePattern), RequestDelegate>? _routes;

        public ApiModule(bool requiredAuth = false)
        {
            this._apis = new List<IApi>();
            this._routes = new Dictionary<(string Method, string RoutePattern), RequestDelegate>();
        }

        public ApiModule(List<IApi> apis, bool requiredAuth = false)
        {
            this._apis = apis;
            this._routes = new Dictionary<(string Method, string RoutePattern), RequestDelegate>();
        }

        /// <inheritdoc />
        public IEnumerable<IApi> APIs => this._apis!;

        /// <inheritdoc />
        public IDictionary<(string Method, string RoutePattern), RequestDelegate> Routes => this._routes!;

        /// <inheritdoc />
        public bool RequireAuth => this._requireAuth;

        /// <inheritdoc />
        public IApiModule AddApi(IApi api)
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }

            this._apis!.Add(api);
            return this;
        }
    }
}
