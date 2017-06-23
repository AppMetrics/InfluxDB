// <copyright file="InfluxDBSettingsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Facts.Client
{
    // ReSharper disable InconsistentNaming
    public class InfluxDBSettingsTests
        // ReSharper restore InconsistentNaming
    {
        [Fact]
        public void Case_address_cannot_be_null()
        {
            Action action = () =>
            {
                var settings = new InfluxDBSettings("influx", null)
                               {
                                   RetensionPolicy = "defaultrp",
                                   Consistenency = "consistency"
                               };
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Can_generate_influx_write_endpoint()
        {
            var settings = new InfluxDBSettings("testdb", new Uri("http://localhost"))
                           {
                               RetensionPolicy = "defaultrp",
                               Consistenency = "consistency"
                           };

            settings.Endpoint.Should().Be("write?db=testdb&rp=defaultrp&consistency=consistency");
        }

        [Fact]
        public void Database_cannot_be_empty()
        {
            Action action = () =>
            {
                var settings = new InfluxDBSettings(string.Empty, new Uri("http://localhost"))
                               {
                                   RetensionPolicy = "defaultrp",
                                   Consistenency = "consistency"
                               };
            };

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Database_cannot_be_null()
        {
            Action action = () =>
            {
                var settings = new InfluxDBSettings(null, new Uri("http://localhost"))
                               {
                                   RetensionPolicy = "defaultrp",
                                   Consistenency = "consistency"
                               };
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Database_cannot_be_whitespace()
        {
            Action action = () =>
            {
                var settings = new InfluxDBSettings(" ", new Uri("http://localhost"))
                               {
                                   RetensionPolicy = "defaultrp",
                                   Consistenency = "consistency"
                               };
            };

            action.ShouldThrow<ArgumentException>();
        }
    }
}