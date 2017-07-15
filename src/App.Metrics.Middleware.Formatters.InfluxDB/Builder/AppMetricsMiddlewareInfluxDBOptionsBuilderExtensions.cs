// <copyright file="AppMetricsMiddlewareInfluxDBOptionsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Builder;
using App.Metrics.Middleware;
using App.Metrics.Middleware.Formatters.InfluxDB;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsMiddlewareInfluxDBOptionsBuilderExtensions
    {
        /// <summary>
        /// Enables InfluxDB's Line Protocol serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics host builder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddMetricsInfluxDBLineProtocolFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, LineProtocolMetricsResponseWriter>());

            return options;
        }

        /// <summary>
        /// Enables InfluxDB's Line Protocol serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics host builder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddMetricsTextInfluxDBLineProtocolFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, LineProtocolTextResponseWriter>());

            return options;
        }

        /// <summary>
        /// Enables InfluxDB's Line Protocol serialization on the metrics and metrics-text responses
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics host builder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddInfluxDBLineProtocolFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AddMetricsInfluxDBLineProtocolFormatters();
            options.AddMetricsTextInfluxDBLineProtocolFormatters();

            return options;
        }
    }
}