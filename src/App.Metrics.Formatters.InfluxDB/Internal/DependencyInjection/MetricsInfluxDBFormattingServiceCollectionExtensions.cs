// <copyright file="MetricsInfluxDBFormattingServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Formatters.InfluxDB.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics InfluxDB formatting services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class MetricsInfluxDBFormattingServiceCollectionExtensions
    {
        internal static void AddLineProtocolFormatterServices(this IServiceCollection services)
        {
            var lineprotocolSetupOptionsDescriptor =
                ServiceDescriptor.Transient<IConfigureOptions<MetricsOptions>, MetricsInfluxDBLineProtocolOptionsSetup>();
            services.TryAddEnumerable(lineprotocolSetupOptionsDescriptor);
        }

        internal static void AddDefaultFormatterOptions(this IServiceCollection services)
        {
            services.Configure<MetricsOptions>(
                options =>
                {
                    if (options.DefaultOutputMetricsFormatter == null)
                    {
                        options.DefaultOutputMetricsFormatter = options.OutputMetricsFormatters.GetType<MetricsInfluxDBLineProtocolOutputFormatter>();
                    }
                });
        }
    }
}