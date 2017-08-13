// <copyright file="MetricsInfluxDBMetricsCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Formatters.InfluxDB.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsInfluxDBMetricsCoreBuilderExtensions
    {
        public static IMetricsCoreBuilder AddInfluxDBLineProtocolFormattersCore(this IMetricsCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddLineProtocolFormatterServices(builder.Services);
            builder.Services.Configure<MetricsOptions>(AddDefaultFormatterOptions);

            return builder;
        }

        internal static void AddLineProtocolFormatterServices(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MetricsOptions>, MetricsInfluxDBLineProtocolOptionsSetup>());
        }

        private static void AddDefaultFormatterOptions(MetricsOptions options)
        {
            if (options.DefaultOutputMetricsTextFormatter == null)
            {
                options.DefaultOutputMetricsTextFormatter = options.OutputMetricsTextFormatters.GetType<MetricsInfluxDBLineProtocolOutputFormatter>();
            }

            if (options.DefaultOutputMetricsFormatter == null)
            {
                options.DefaultOutputMetricsFormatter = options.OutputMetricsFormatters.GetType<MetricsInfluxDBLineProtocolOutputFormatter>();
            }
        }
    }
}
