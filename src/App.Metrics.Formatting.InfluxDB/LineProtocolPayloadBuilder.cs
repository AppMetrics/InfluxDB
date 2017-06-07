// <copyright file="LineProtocolPayloadBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Formatting.InfluxDB
{
    public class LineProtocolPayloadBuilder : IMetricPayloadBuilder<LineProtocolPayload>
    {
        private readonly Func<string, string, string> _metricNameFormatter;
        private LineProtocolPayload _payload;

        public LineProtocolPayloadBuilder(MetricValueDataKeys dataKeys = null, Func<string, string, string> metricNameFormatter = null)
        {
            _payload = new LineProtocolPayload();

            DataKeys = dataKeys ?? new MetricValueDataKeys();
            _metricNameFormatter = metricNameFormatter ?? Constants.InfluxDBDefaults.MetricNameFormatter;
        }

        /// <inheritdoc />
        public MetricValueDataKeys DataKeys { get; }

        /// <inheritdoc />
        public void Clear() { _payload = null; }

        /// <inheritdoc />
        public void Init() { _payload = new LineProtocolPayload(); }

        /// <inheritdoc />
        public void Pack(string context, string name, object value, MetricTags tags)
        {
            var measurement = _metricNameFormatter(context, name);
            _payload.Add(new LineProtocolPoint(measurement, new Dictionary<string, object> { { "value", value } }, tags));
        }

        /// <inheritdoc />
        public void Pack(
            string context,
            string name,
            IEnumerable<string> columns,
            IEnumerable<object> values,
            MetricTags tags)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            var measurement = _metricNameFormatter(context, name);
            _payload.Add(new LineProtocolPoint(measurement, fields, tags));
        }

        /// <inheritdoc />
        public LineProtocolPayload Payload() { return _payload; }

        /// <inheritdoc />
        public string PayloadFormatted()
        {
            var result = new StringWriter();
            _payload.Format(result);
            return result.ToString();
        }

        public string PayloadFormatted(bool writeTimestamp)
        {
            var result = new StringWriter();
            _payload.Format(result, writeTimestamp);
            return result.ToString();
        }
    }
}