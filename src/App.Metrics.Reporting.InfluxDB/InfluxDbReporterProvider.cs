// <copyright file="InfluxDbReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Filtering;
using App.Metrics.Filters;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Reporting.InfluxDB.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace App.Metrics.Reporting.InfluxDB
{
    public class InfluxDbReporterProvider : IReporterProvider
    {
        private readonly InfluxDBReporterSettings _settings;

        public InfluxDbReporterProvider(InfluxDBReporterSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            Filter = new NoOpMetricsFilter();
        }

        public InfluxDbReporterProvider(InfluxDBReporterSettings settings, IFilterMetrics filter)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            Filter = filter ?? new NoOpMetricsFilter();
        }

        public IFilterMetrics Filter { get; }

        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory)
        {
            var lineProtocolClient = new DefaultLineProtocolClient(
                loggerFactory,
                _settings.InfluxDbSettings,
                _settings.HttpPolicy);
            var payloadBuilder = new LineProtocolPayloadBuilder(_settings.DataKeys, _settings.MetricNameFormatter);

            return new ReportRunner<LineProtocolPayload>(
                async p =>
                {
                    var result = await lineProtocolClient.WriteAsync(p.Payload());
                    return result.Success;
                },
                payloadBuilder,
                _settings.ReportInterval,
                name,
                loggerFactory);
        }
    }
}