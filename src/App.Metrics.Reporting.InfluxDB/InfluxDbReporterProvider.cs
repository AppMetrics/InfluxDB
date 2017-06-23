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
        private readonly ILoggerFactory _loggerFactory;
        private readonly InfluxDBReporterSettings _settings;

        public InfluxDbReporterProvider(InfluxDBReporterSettings settings, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            Filter = new NoOpMetricsFilter();
        }

        public InfluxDbReporterProvider(InfluxDBReporterSettings settings, ILoggerFactory loggerFactory, IFilterMetrics fitler)
        {
            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            Filter = fitler ?? new NoOpMetricsFilter();
        }

        public IFilterMetrics Filter { get; }

        public IMetricReporter CreateMetricReporter(string name)
        {
            var lineProtocolClient = new DefaultLineProtocolClient(
                _loggerFactory,
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
                _loggerFactory);
        }
    }
}