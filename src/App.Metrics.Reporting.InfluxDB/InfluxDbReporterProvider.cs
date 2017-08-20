// <copyright file="InfluxDbReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Reporting.InfluxDB.Client;
using Microsoft.Extensions.Options;

namespace App.Metrics.Reporting.InfluxDB
{
    public class InfluxDbReporterProvider : IReporterProvider
    {
        private readonly IOptions<MetricsReportingInfluxDBOptions> _influxDbOptionsAccessor;
        private readonly ILineProtocolClient _lineProtocolClient;

        public InfluxDbReporterProvider(
            IOptions<MetricsReportingOptions> optionsAccessor,
            IOptions<MetricsReportingInfluxDBOptions> influxDbReportingOptionsAccessor,
            ILineProtocolClient lineProtocolClient)
        {
            _influxDbOptionsAccessor = influxDbReportingOptionsAccessor ?? throw new ArgumentNullException(nameof(influxDbReportingOptionsAccessor));
            _lineProtocolClient = lineProtocolClient ?? throw new ArgumentNullException(nameof(lineProtocolClient));
            Filter = influxDbReportingOptionsAccessor.Value.Filter ?? optionsAccessor.Value.Filter;
            ReportInterval = influxDbReportingOptionsAccessor.Value.ReportInterval;
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var stream = new MemoryStream())
            {
                await _influxDbOptionsAccessor.Value.MetricsOutputFormatter.WriteAsync(stream, metricsData, cancellationToken);

                await _lineProtocolClient.WriteAsync(Encoding.UTF8.GetString(stream.ToArray()), cancellationToken);
            }

            return true;
        }
    }
}