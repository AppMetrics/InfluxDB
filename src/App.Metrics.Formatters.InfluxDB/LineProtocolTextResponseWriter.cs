// <copyright file="LineProtocolTextResponseWriter.cs" company="Allan Hardy">
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
    public class LineProtocolTextResponseWriter : IMetricsTextResponseWriter
    {
        /// <inheritdoc />
        public string ContentType => "text/plain; app.metrics=vnd.app.metrics.v1.metrics.influxdb; influx=lineprotocol-1.2.x;";

        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken))
        {
            var payloadBuilder = new LineProtocolPayloadBuilder();

            var builder = new MetricDataValueSourceFormatter();
            builder.Build(metricsData, payloadBuilder);

            return context.Response.WriteAsync(payloadBuilder.PayloadFormatted(), token);
        }
    }
}