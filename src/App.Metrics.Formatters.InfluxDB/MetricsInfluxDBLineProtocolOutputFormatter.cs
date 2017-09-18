// <copyright file="MetricsInfluxDbLineProtocolOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
#if !NETSTANDARD1_6
using App.Metrics.Internal;
#endif
using App.Metrics.Serialization;

namespace App.Metrics.Formatters.InfluxDB
{
    public class MetricsInfluxDbLineProtocolOutputFormatter : IMetricsOutputFormatter
    {
        private readonly MetricsInfluxDbLineProtocolOptions _options;

        public MetricsInfluxDbLineProtocolOutputFormatter()
        {
            _options = new MetricsInfluxDbLineProtocolOptions();
        }

        public MetricsInfluxDbLineProtocolOutputFormatter(MetricsInfluxDbLineProtocolOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

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
                using (var textWriter = new MetricSnapshotInfluxDbLineProtocolWriter(
                    streamWriter,
                    _options.MetricNameFormatter,
                    _options.MetricNameMapping))
                {
                    serializer.Serialize(textWriter, metricsData);
                }
            }

#if !NETSTANDARD1_6
            return AppMetricsTaskHelper.CompletedTask();
#else
            return Task.CompletedTask;
#endif
        }
    }
}