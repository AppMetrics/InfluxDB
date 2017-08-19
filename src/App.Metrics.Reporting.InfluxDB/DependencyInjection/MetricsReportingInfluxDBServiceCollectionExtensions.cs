// <copyright file="MetricsReportingInfluxDBServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using App.Metrics;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Console;
using App.Metrics.Reporting.InfluxDB;
using App.Metrics.Reporting.InfluxDB.Client;
using App.Metrics.Reporting.InfluxDB.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up essential App Metrics console reporting services in an
    ///     <see cref="IServiceCollection" />.
    /// </summary>
    public static class MetricsReportingInfluxDBServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds Essential App Metrics influxdb reporting metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="influxBaseUri">The base URI of the InfluxDB API.</param>
        /// <param name="influxDatabase">The InfluxDB database name used to report metrics.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure the App Metrics services.
        /// </returns>
        internal static IServiceCollection AddInfluxDBCore(
            this IServiceCollection services,
            Uri influxBaseUri,
            string influxDatabase)
        {
            AddInfluxDBReportingServices(services, influxBaseUri, influxDatabase);

            return services;
        }

        /// <summary>
        ///     Adds Essential App Metrics influxdb reporting metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsReportingInfluxDBOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure the App Metrics services.
        /// </returns>
        internal static IServiceCollection AddInfluxDBCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var influxOptions = new MetricsReportingInfluxDBOptions();
            configuration.Bind(nameof(MetricsReportingInfluxDBOptions), influxOptions);

            AddInfluxDBReportingServices(services, influxOptions.InfluxDB.InfluxBaseUri, influxOptions.InfluxDB.InfluxDatabase);

            return services;
        }

        internal static void AddInfluxDBReportingServices(IServiceCollection services, Uri influxBaseUri, string influxDatabase)
        {
            if (influxBaseUri == default(Uri))
            {
                throw new InvalidOperationException(
                    "MetricsReportingInfluxDBOptions.InfluxDB.InfluxBaseUri is required, check the application's startup code and/or configuration");
            }

            if (string.IsNullOrWhiteSpace(influxDatabase))
            {
                throw new InvalidOperationException(
                    "MetricsReportingInfluxDBOptions.InfluxDB.InfluxDatabase is required, check the application's startup code and/or configuration");
            }

            //
            // Options
            //
            var optionsSetupDescriptor =
                ServiceDescriptor.Transient<IConfigureOptions<MetricsReportingInfluxDBOptions>, MetricsReportingInfluxDBOptionsSetup>(
                    provider =>
                    {
                        var optionsAccessor = provider.GetRequiredService<IOptions<MetricsOptions>>();
                        return new MetricsReportingInfluxDBOptionsSetup(optionsAccessor, influxBaseUri, influxDatabase);
                    });

            services.TryAddEnumerable(optionsSetupDescriptor);

            //
            // InfluxDB Reporting Infrastructure
            //
            var consoleReportProviderDescriptor = ServiceDescriptor.Transient<IReporterProvider, InfluxDbReporterProvider>();
            services.TryAddEnumerable(consoleReportProviderDescriptor);
            services.TryAddSingleton<ILineProtocolClient>(
                provider =>
                {
                    var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                    var optionsAccessor = provider.GetRequiredService<IOptions<MetricsReportingInfluxDBOptions>>();
                    var httpClient = CreateHttpClient(optionsAccessor.Value.InfluxDB, optionsAccessor.Value.HttpPolicy);

                    return new DefaultLineProtocolClient(
                        loggerFactory.CreateLogger<DefaultLineProtocolClient>(),
                        optionsAccessor.Value.InfluxDB,
                        optionsAccessor.Value.HttpPolicy,
                        httpClient);
                });
        }

        internal static HttpClient CreateHttpClient(
            InfluxDBOptions influxDbOptions,
            HttpPolicy httpPolicy,
            HttpMessageHandler httpMessageHandler = null)
        {
            var client = httpMessageHandler == null
                ? new HttpClient()
                : new HttpClient(httpMessageHandler);

            client.BaseAddress = influxDbOptions.InfluxBaseUri;
            client.Timeout = httpPolicy.Timeout;

            if (string.IsNullOrWhiteSpace(influxDbOptions.UserName) || string.IsNullOrWhiteSpace(influxDbOptions.Password))
            {
                return client;
            }

            var byteArray = Encoding.ASCII.GetBytes($"{influxDbOptions.UserName}:{influxDbOptions.Password}");
            client.BaseAddress = influxDbOptions.InfluxBaseUri;
            client.Timeout = httpPolicy.Timeout;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return client;
        }
    }
}