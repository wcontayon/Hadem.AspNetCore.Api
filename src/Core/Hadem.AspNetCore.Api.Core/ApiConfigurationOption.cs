// Copyright (c) {Hadem.AspNetCore.Api}. All rights reserved.

namespace Hadem.AspNetCore.Api.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Define option to be use to configure all the <see cref="IApi"/>.
    /// </summary>
    public class ApiConfigurationOption
    {
        /// <summary>
        /// Gets or Sets a value indicating whether to use Authorization on all <see cref="IApi"/>.
        /// </summary>
        public bool UseAuthorization { get; set; }
    }
}
