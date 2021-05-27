// Copyright (c) {Hadem.AspNetCore.Api}. All rights reserved.

namespace Hadem.AspNetCore.Api.Core.Builder
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ApiModuleServiceCollectionExtensions
    {
        /// <summary>
        /// Add the <see cref="IApiModule"/> to the service configuration.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        public static IServiceCollection AddApiModule(this ServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Add the markerservice
            services.TryAddSingleton<ApiModuleMarkerService, ApiModuleMarkerService>();

            // Create the ApiModule
            services.TryAddSingleton<IApiModule, ApiModule>();

            return services;
        }

        /// <summary>
        /// Add the <see cref="IApiModule"/> to the service configuration.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="option"><see cref="ApiConfigurationOption"/> to use.</param>
        public static IServiceCollection AddApiModule(
            this IServiceCollection services,
            ApiConfigurationOption option)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Add the markerservice
            services.TryAddSingleton<ApiModuleMarkerService, ApiModuleMarkerService>();

            // Create the ApiModule
            services.TryAddSingleton<IApiModule>(s => new ApiModule(option.UseAuthorization));

            return services;
        }

        /// <summary>
        /// Add a scoped <see cref="IApi"/> to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TApi">The <see cref="IApi"/> class.</typeparam>
        /// <param name="instance">The <see cref="IApi"/> instance.</param>
        public static IServiceCollection AddScopedApi<TApi>(this IServiceCollection services, TApi instance)
            where TApi : class, IApi
        {
            if (instance == null)
            {
                throw new InvalidOperationException("Cannot add a null instance");
            }

            services.TryAddScoped<TApi>(s =>
            {
                var apiModule = EnsureApiModuleRegistered(s);
                apiModule.AddApi(instance);
                return instance;
            });

            return services;
        }

        /// <summary>
        /// Add a scoped <see cref="IApi"/> to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TApi">The <see cref="IApi"/> class.</typeparam>
        public static IServiceCollection AddScopedApi<TApi>(
            this IServiceCollection services,
            Func<IServiceProvider, TApi> instanceImplementation)
            where TApi : class, IApi
        {
            services.TryAddScoped<TApi>(s =>
            {
                var apiModule = EnsureApiModuleRegistered(s);
                var instance = instanceImplementation(s);
                if (instance == null)
                {
                    throw new InvalidOperationException("Cannot add a null instance");
                }

                apiModule.AddApi(instance);
                return instance;
            });

            return services;
        }

        private static IApiModule EnsureApiModuleRegistered(IServiceProvider serviceProvider)
        {
            var apiModule = serviceProvider.GetRequiredService<IApiModule>();
            if (apiModule == null)
            {
                throw new InvalidOperationException("Use {IServiceCollection.AddApiModule} before add any IApi to the IServiceCollection");
            }

            return apiModule;
        }
    }
}
