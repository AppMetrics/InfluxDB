// <copyright file="MetricsReportingInfluxDBMetricsReportingBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Formatters.InfluxDB.Internal;
using App.Metrics.Reporting.InfluxDB;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsReportingInfluxDBMetricsReportingBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics influxdb reporting metrics services to the specified <see cref="IMetricsReportingBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <param name="influxBaseUri">The base URI of the InfluxDB API.</param>
        /// <param name="influxDatabase">The InfluxDB database name used to report metrics.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure the App Metrics influxdb reporting services.
        /// </returns>
        public static IMetricsReportingBuilder AddInfluxDB(
            this IMetricsReportingBuilder builder,
            Uri influxBaseUri,
            string influxDatabase)
        {
            // TODO: Refacor
            MetricsInfluxDBMetricsCoreBuilderExtensions.AddLineProtocolFormatterServices(builder.Services);

            builder.Services.AddInfluxDBCore(influxBaseUri, influxDatabase);

            return builder;
        }

        /// <summary>
        ///     Adds App Metrics influxdb reporting metrics services to the specified <see cref="IMetricsReportingBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <param name="influxBaseUri">The base URI of the InfluxDB API.</param>
        /// <param name="influxDatabase">The InfluxDB database name used to report metrics.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsReportingInfluxDBOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure the App Metrics influxdb reporting services.
        /// </returns>
        public static IMetricsReportingBuilder AddInfluxDB(
            this IMetricsReportingBuilder builder,
            Uri influxBaseUri,
            string influxDatabase,
            Action<MetricsReportingInfluxDBOptions> setupAction)
        {
            var reportingBuilder = builder.AddInfluxDB(influxBaseUri, influxDatabase);

            builder.Services.Configure(setupAction);

            return reportingBuilder;
        }

        /// <summary>
        ///     Adds App Metrics influxdb reporting metrics services to the specified <see cref="IMetricsReportingCoreBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingCoreBuilder" /> to add services to.</param>
        /// <param name="influxBaseUri">The base URI of the InfluxDB API.</param>
        /// <param name="influxDatabase">The InfluxDB database name used to report metrics.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics influxdb reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddInfluxDB(
            this IMetricsReportingCoreBuilder builder,
            Uri influxBaseUri,
            string influxDatabase)
        {
            builder.Services.AddInfluxDBCore(influxBaseUri, influxDatabase);

            return builder;
        }

        /// <summary>
        ///     Adds App Metrics influxdb reporting metrics services to the specified <see cref="IMetricsReportingCoreBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingCoreBuilder" /> to add services to.</param>
        /// <param name="influxBaseUri">The base URI of the InfluxDB API.</param>
        /// <param name="influxDatabase">The InfluxDB database name used to report metrics.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsReportingInfluxDBOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics influxdb reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddInfluxDB(
            this IMetricsReportingCoreBuilder builder,
            Uri influxBaseUri,
            string influxDatabase,
            Action<MetricsReportingInfluxDBOptions> setupAction)
        {
            var reportingBuilder = builder.AddInfluxDB(influxBaseUri, influxDatabase);

            builder.Services.Configure(setupAction);

            return reportingBuilder;
        }
    }
}