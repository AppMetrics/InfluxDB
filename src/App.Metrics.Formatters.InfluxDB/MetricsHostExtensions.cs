// <copyright file="MetricsHostExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Middleware;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsHostExtensions
    {
        /// <summary>
        /// Enables InfluxDB's Line Protocol serialization on the metric endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddInfluxDBLineProtocolMetricsSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, LineProtocolMetricsResponseWriter>());

            return host;
        }

        /// <summary>
        /// Enables InfluxDB's Line Protocol serialization on the metric endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddInfluxDBLineProtocolMetricsTextSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, LineProtocolTextResponseWriter>());

            return host;
        }

        /// <summary>
        /// Enables InfluxDB's Line Protocol serialization on the metrics and metrics-text responses
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddInfluxDBLineProtocolSerialization(this IMetricsHostBuilder host)
        {
            host.AddInfluxDBLineProtocolMetricsSerialization();
            host.AddInfluxDBLineProtocolMetricsTextSerialization();
            return host;
        }
    }
}