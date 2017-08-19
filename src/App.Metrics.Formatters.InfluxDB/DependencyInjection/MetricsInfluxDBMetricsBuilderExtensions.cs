// <copyright file="MetricsInfluxDBMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.InfluxDB;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics InfluxDB formatting services in an <see cref="IMetricsBuilder" />.
    /// </summary>
    public static class MetricsInfluxDBMetricsBuilderExtensions
    {
        /// <summary>
        ///     Adds InfluxDB LineProtocol formatters to the specified <see cref="IMetricsBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <returns>An <see cref="IMetricsBuilder"/> that can be used to further configure the App Metrics services.</returns>
        public static IMetricsBuilder AddInfluxDBLineProtocolFormatters(this IMetricsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddLineProtocolFormatterServices();
            builder.Services.AddDefaultFormatterOptions();

            return builder;
        }

        /// <summary>
        ///     Adds InfluxDB LineProtocol options to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsInfluxDBLineProtocolOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsBuilder AddInfluxDBLineProtocolOptions(
            this IMetricsBuilder builder,
            Action<MetricsInfluxDBLineProtocolOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure(setupAction);

            return builder;
        }
    }
}