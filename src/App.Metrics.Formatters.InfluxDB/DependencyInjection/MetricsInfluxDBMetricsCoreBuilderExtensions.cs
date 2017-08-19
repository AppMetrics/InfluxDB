// <copyright file="MetricsInfluxDBMetricsCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.InfluxDB;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics InfluxDB formatting services in an <see cref="IMetricsCoreBuilder" />.
    /// </summary>
    public static class MetricsInfluxDBMetricsCoreBuilderExtensions
    {
        /// <summary>
        ///     Adds InfluxDB LineProtocol formatters to the specified <see cref="IMetricsCoreBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsCoreBuilder" /> to add services to.</param>
        /// <returns>An <see cref="IMetricsCoreBuilder"/> that can be used to further configure App Metrics services.</returns>
        public static IMetricsCoreBuilder AddInfluxDBLineProtocolFormattersCore(this IMetricsCoreBuilder builder)
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
        ///     Adds InfluxDB LineProtocol options to the specified <see cref="IMetricsCoreBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsCoreBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsInfluxDBLineProtocolOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsCoreBuilder" /> that can be used to further configure App Metrics services.
        /// </returns>
        public static IMetricsCoreBuilder AddInfluxDBLineProtocolOptionsCore(
            this IMetricsCoreBuilder builder,
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
