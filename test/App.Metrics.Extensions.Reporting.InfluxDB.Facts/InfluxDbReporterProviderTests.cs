// <copyright file="InfluxDbReporterProviderTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Filtering;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Facts
{
    public class InfluxDbReporterProviderTests
    {
        [Fact]
        public void Can_create_metric_reporter()
        {
            var provider = new InfluxDbReporterProvider(new InfluxDBReporterSettings(), new LoggerFactory(), new DefaultMetricsFilter());

            var reporter = provider.CreateMetricReporter("influx");

            reporter.Should().NotBeNull();
        }

        [Fact]
        public void Defaults_filter_to_no_op()
        {
            var provider = new InfluxDbReporterProvider(new InfluxDBReporterSettings(), new LoggerFactory());

            provider.Filter.Should().BeOfType<NoOpMetricsFilter>();
        }

        [Fact]
        public void Filter_is_not_required()
        {
            Action action = () =>
            {
                var provider = new InfluxDbReporterProvider(new InfluxDBReporterSettings(), null);
                provider.Filter.Should().BeOfType<NoOpMetricsFilter>();
            };

            action.ShouldNotThrow();
        }

        [Fact]
        public void Settings_are_required()
        {
            Action action = () =>
            {
                var provider = new InfluxDbReporterProvider(null, new LoggerFactory());
            };

            action.ShouldThrow<ArgumentNullException>();
        }
    }
}