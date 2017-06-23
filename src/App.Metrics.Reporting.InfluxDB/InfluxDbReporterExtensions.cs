// <copyright file="InfluxDbReporterExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filters;
using App.Metrics.Reporting.InfluxDB;
using App.Metrics.Reporting.InfluxDB.Client;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Reporting.Interfaces
    // ReSharper restore CheckNamespace
{
    public static class InfluxDbReporterExtensions
    {
        public static IReportFactory AddInfluxDb(
            this IReportFactory factory,
            InfluxDBReporterSettings settings,
            ILoggerFactory loggerFactory,
            IFilterMetrics filter = null)
        {
            factory.AddProvider(new InfluxDbReporterProvider(settings, loggerFactory, filter));
            return factory;
        }

        public static IReportFactory AddInfluxDb(
            this IReportFactory factory,
            string database,
            Uri baseAddress,
            ILoggerFactory loggerFactory,
            IFilterMetrics filter = null)
        {
            var settings = new InfluxDBReporterSettings
                           {
                               InfluxDbSettings = new InfluxDBSettings(database, baseAddress)
                           };

            factory.AddInfluxDb(settings, loggerFactory, filter);
            return factory;
        }
    }
}