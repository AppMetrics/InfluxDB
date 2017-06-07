// <copyright file="LineProtocolMetricsResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Extensions.Middleware.Abstractions;
using App.Metrics.Formatting;
using App.Metrics.Formatting.InfluxDB;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.Formatters.InfluxDB
{
    public class LineProtocolMetricsResponseWriter : IMetricsResponseWriter
    {
        /// <inheritdoc />
        public string ContentType => "application/vnd.app.metrics.v1.metrics.influxdb; influx=lineprotocol-1.2.x;";

        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken))
        {
            var payloadBuilder = new LineProtocolPayloadBuilder();

            var formatter = new MetricDataValueSourceFormatter();
            formatter.Build(metricsData, payloadBuilder);

            return context.Response.WriteAsync(payloadBuilder.PayloadFormatted(), token);
        }
    }
}