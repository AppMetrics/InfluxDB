// <copyright file="MetricsReportingInfluxDBOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.InfluxDB;
using Microsoft.Extensions.Options;

namespace App.Metrics.Reporting.InfluxDB.Internal
{
    /// <summary>
    ///     Sets up default InfluxDB Reporting options for <see cref="MetricsReportingInfluxDBOptions"/>.
    /// </summary>
    public class MetricsReportingInfluxDBOptionsSetup : IConfigureOptions<MetricsReportingInfluxDBOptions>
    {
        private readonly Uri _influxBaseUri;
        private readonly string _influxDatabase;
        private readonly MetricsOptions _metricsOptionsAccessor;

        public MetricsReportingInfluxDBOptionsSetup(IOptions<MetricsOptions> metricsOptionsAccessor, Uri influxBaseUri, string influxDatabase)
        {
            if (string.IsNullOrWhiteSpace(influxDatabase))
            {
                throw new ArgumentException("An InfluxDB Database name is required.", nameof(influxDatabase));
            }

            _influxBaseUri = influxBaseUri ?? throw new ArgumentNullException(nameof(influxBaseUri));
            _influxDatabase = influxDatabase;
            _metricsOptionsAccessor = metricsOptionsAccessor.Value ?? throw new ArgumentNullException(nameof(metricsOptionsAccessor));
        }

        /// <inheritdoc/>
        public void Configure(MetricsReportingInfluxDBOptions options)
        {
            options.InfluxDB.BaseUri = _influxBaseUri;
            options.InfluxDB.Database = _influxDatabase;

            if (options.MetricsOutputFormatter == null)
            {
                options.MetricsOutputFormatter = _metricsOptionsAccessor.OutputMetricsFormatters.GetType<MetricsInfluxDBLineProtocolOutputFormatter>();
            }
        }
    }
}