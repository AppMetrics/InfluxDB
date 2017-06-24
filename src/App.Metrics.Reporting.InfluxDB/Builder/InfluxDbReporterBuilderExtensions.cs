// <copyright file="InfluxDbReporterBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Filtering;
using App.Metrics.Filters;
using App.Metrics.Reporting;
using App.Metrics.Reporting.InfluxDB;
using App.Metrics.Reporting.InfluxDB.Client;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class InfluxDbReporterBuilderExtensions
    {
        public static IReportFactory AddInfluxDb(
            this IReportFactory factory,
            InfluxDBReporterSettings settings,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            factory.AddProvider(new InfluxDbReporterProvider(settings, filter));
            return factory;
        }

        public static IReportFactory AddInfluxDb(
            this IReportFactory factory,
            string database,
            Uri baseAddress,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            var settings = new InfluxDBReporterSettings
                           {
                               InfluxDbSettings = new InfluxDBSettings(database, baseAddress)
                           };

            factory.AddInfluxDb(settings, filter);
            return factory;
        }
    }
}