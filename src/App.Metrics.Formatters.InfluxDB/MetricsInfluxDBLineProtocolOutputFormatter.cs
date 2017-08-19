// <copyright file="MetricsInfluxDBLineProtocolOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Serialization;

namespace App.Metrics.Formatters.InfluxDB
{
    public class MetricsInfluxDBLineProtocolOutputFormatter : IMetricsOutputFormatter
    {
        private readonly MetricsInfluxDBLineProtocolOptions _options;

        public MetricsInfluxDBLineProtocolOutputFormatter()
        {
            _options = new MetricsInfluxDBLineProtocolOptions();
        }

        public MetricsInfluxDBLineProtocolOutputFormatter(MetricsInfluxDBLineProtocolOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

        /// <inheritdoc/>
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics.influxdb", "v1", "plain");

        /// <inheritdoc/>
        public Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var serializer = new MetricSnapshotSerializer();

            using (var streamWriter = new StreamWriter(output))
            {
                using (var textWriter = new MetricSnapshotInfluxDBLineProtocolWriter(
                    streamWriter,
                    _options.MetricNameFormatter,
                    _options.MetricNameMapping))
                {
                    serializer.Serialize(textWriter, metricsData);
                }
            }

            return Task.CompletedTask;
        }
    }
}