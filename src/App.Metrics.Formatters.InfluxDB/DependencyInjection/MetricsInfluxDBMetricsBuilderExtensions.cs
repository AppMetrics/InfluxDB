// <copyright file="MetricsInfluxDBMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.InfluxDB;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsInfluxDBMetricsBuilderExtensions
    {
        public static IMetricsBuilder AddInfluxDBLineProtocolOptions(
            this IMetricsBuilder builder,
            Action<MetricsInfluxDBLineProtocolOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure(setupAction);

            return builder;
        }
    }
}