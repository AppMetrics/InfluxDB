// <copyright file="InfluxDbReporterExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Configuration;
using App.Metrics.Core.Filtering;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Facts.Extensions
{
    public class InfluxDbReporterExtensionsTests
    {
        [Fact]
        public void Can_add_influxdb_provider_with_custom_settings()
        {
            var factory = SetupReportFactory();
            var settings = new InfluxDBReporterSettings
                           {
                               HttpPolicy = new HttpPolicy
                                            {
                                                BackoffPeriod = TimeSpan.FromMinutes(1)
                                            }
                           };
            Action action = () => { factory.AddInfluxDb(settings, new LoggerFactory()); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void Can_add_influxdb_provider_with_custom_settings_and_filter()
        {
            var factory = SetupReportFactory();

            var settings = new InfluxDBReporterSettings
                           {
                               HttpPolicy = new HttpPolicy
                                            {
                                                BackoffPeriod = TimeSpan.FromMinutes(1)
                                            }
                           };
            Action action = () => { factory.AddInfluxDb(settings, new LoggerFactory(), new DefaultMetricsFilter()); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void Can_add_influxdb_provider_with_filter()
        {
            var factory = SetupReportFactory();

            Action action = () => { factory.AddInfluxDb("test", new Uri("http://localhost"), new LoggerFactory(), new DefaultMetricsFilter()); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void Can_add_influxdb_provider_without_filter()
        {
            var factory = SetupReportFactory();

            Action action = () => { factory.AddInfluxDb("test", new Uri("http://localhost"), new LoggerFactory()); };

            action.ShouldNotThrow();
        }

        private static ReportFactory SetupReportFactory()
        {
            var metricsMock = new Mock<IMetrics>();
            var options = new AppMetricsOptions();
            return new ReportFactory(metricsMock.Object, new LoggerFactory());
        }
    }
}