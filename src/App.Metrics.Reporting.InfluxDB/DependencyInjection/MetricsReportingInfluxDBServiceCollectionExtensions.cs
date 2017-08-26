// <copyright file="MetricsReportingInfluxDBServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using App.Metrics;
using App.Metrics.Reporting;
using App.Metrics.Reporting.InfluxDB;
using App.Metrics.Reporting.InfluxDB.Client;
using App.Metrics.Reporting.InfluxDB.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up essential App Metrics InfluxDB reporting services in an
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

            AddInfluxDBReportingServices(services, influxOptions.InfluxDB.BaseUri, influxOptions.InfluxDB.Database);

            return services;
        }

        internal static void AddInfluxDBReportingServices(IServiceCollection services, Uri influxBaseUri, string influxDatabase)
        {
            if (influxBaseUri == default(Uri))
            {
                throw new InvalidOperationException(
                    "MetricsReportingInfluxDBOptions.InfluxDB.BaseUri is required, check the application's startup code and/or configuration");
            }

            if (string.IsNullOrWhiteSpace(influxDatabase))
            {
                throw new InvalidOperationException(
                    "MetricsReportingInfluxDBOptions.InfluxDB.Database is required, check the application's startup code and/or configuration");
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
            var influxDbReportProviderDescriptor = ServiceDescriptor.Transient<IReporterProvider, InfluxDbReporterProvider>();
            services.TryAddEnumerable(influxDbReportProviderDescriptor);
            services.TryAddSingleton<ILineProtocolClient>(
                provider =>
                {
                    var optionsAccessor = provider.GetRequiredService<IOptions<MetricsReportingInfluxDBOptions>>();
                    var httpClient = CreateHttpClient(optionsAccessor.Value.InfluxDB, optionsAccessor.Value.HttpPolicy);

                    return new DefaultLineProtocolClient(
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

            client.BaseAddress = influxDbOptions.BaseUri;
            client.Timeout = httpPolicy.Timeout;

            if (string.IsNullOrWhiteSpace(influxDbOptions.UserName) || string.IsNullOrWhiteSpace(influxDbOptions.Password))
            {
                return client;
            }

            var byteArray = Encoding.ASCII.GetBytes($"{influxDbOptions.UserName}:{influxDbOptions.Password}");
            client.BaseAddress = influxDbOptions.BaseUri;
            client.Timeout = httpPolicy.Timeout;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return client;
        }
    }
}