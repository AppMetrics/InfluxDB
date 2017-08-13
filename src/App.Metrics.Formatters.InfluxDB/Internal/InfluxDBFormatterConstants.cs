// <copyright file="InfluxDBFormatterConstants.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    public static class InfluxDBFormatterConstants
    {
        public class LineProtocol
        {
            public static readonly Func<string, string, string> MetricNameFormatter =
                (metricContext, metricName) => string.IsNullOrWhiteSpace(metricContext)
                    ? $"{metricName}".Replace(' ', '_').ToLowerInvariant()
                    : $"{metricContext}__{metricName}".Replace(' ', '_').ToLowerInvariant();
        }
    }
}