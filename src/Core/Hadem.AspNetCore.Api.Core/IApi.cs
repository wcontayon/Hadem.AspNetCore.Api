// Copyright (c) {Hadem.AspNetCore.Api}. All rights reserved.

namespace Hadem.AspNetCore.Api
{
    using System;

    /// <summary>
    /// Interface that specify that a class i an Api to be exposed.
    /// </sumarry>
    public interface IApi
    {
        /// <summary>
        /// Gets or Sets the Name of the <see cref="IApi"/>.
        /// <summary>
        public string ApiName { get; set; }

        /// <summary>
        /// Gets or Sets the prefix to used in the Api routing.
        /// </summary>
        public string RoutePrefix { get; set; }
    }
}