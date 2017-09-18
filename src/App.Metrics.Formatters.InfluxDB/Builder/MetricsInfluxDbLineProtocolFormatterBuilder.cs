// <copyright file="MetricsInfluxDbLineProtocolFormatterBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.InfluxDB;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricsInfluxDbLineProtocolFormatterBuilder
    {
        /// <summary>
        ///     Add the <see cref="MetricsInfluxDbLineProtocolOutputFormatter" /> allowing metrics to optionally be reported to
        ///     InfluxDB using the LineProtocol.
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure JSON formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The InfluxDB LineProtocol formatting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsInfluxDbLineProtocol(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            Action<MetricsInfluxDbLineProtocolOptions> setupAction)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var options = new MetricsInfluxDbLineProtocolOptions();

            setupAction.Invoke(options);

            var formatter = new MetricsInfluxDbLineProtocolOutputFormatter(options);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsInfluxDbLineProtocolOutputFormatter" /> allowing metrics to optionally be reported to
        ///     InfluxDB using the LineProtocol.
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure JSON formatting
        ///     options.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsInfluxDbLineProtocol(this IMetricsOutputFormattingBuilder metricFormattingBuilder)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsInfluxDbLineProtocolOutputFormatter();

            return metricFormattingBuilder.Using(formatter, false);
        }
    }
}