// <copyright file="MetricsInfluxDBLineProtocolOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    /// <summary>
    ///     Sets up default InfluxDB Line Protocol options for <see cref="MetricsOptions"/>.
    /// </summary>
    public class MetricsInfluxDBLineProtocolOptionsSetup : IConfigureOptions<MetricsOptions>
    {
        private readonly MetricsInfluxDBLineProtocolOptions _lineProtocolOptions;

        public MetricsInfluxDBLineProtocolOptionsSetup(IOptions<MetricsInfluxDBLineProtocolOptions> asciiOptionsAccessor)
        {
            _lineProtocolOptions = asciiOptionsAccessor.Value ?? throw new ArgumentNullException(nameof(asciiOptionsAccessor));
        }

        public void Configure(MetricsOptions options)
        {
            var formatter = new MetricsInfluxDBLineProtocolOutputFormatter(_lineProtocolOptions);

            options.OutputMetricsFormatters.Add(formatter);
            options.OutputMetricsTextFormatters.Add(formatter);
        }
    }
}