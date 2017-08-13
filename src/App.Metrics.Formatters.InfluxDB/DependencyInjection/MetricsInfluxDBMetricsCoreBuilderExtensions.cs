// <copyright file="MetricsInfluxDBMetricsCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Formatters.InfluxDB.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsInfluxDBMetricsCoreBuilderExtensions
    {
        public static IMetricsCoreBuilder AddInfluxDBLineProtocolFormatters(this IMetricsCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddTextFormatterServices(builder.Services);

            return builder;
        }

        internal static void AddTextFormatterServices(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MetricsOptions>, MetricsInfluxDBLineProtocolOptionsSetup>());
        }
    }
}
