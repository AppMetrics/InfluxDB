// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsInfluxDBSandboxMvc
{
    public static class Host
    {
        private static readonly string InfluxDbDatabase = "appmetricssandbox";
        private static readonly Uri InfluxDbUri = new Uri("http://127.0.0.1:8086");

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                           .UseMetrics()
                           .UseMetricsReporting(
                               reportingBuilder =>
                               {
                                   reportingBuilder.AddInfluxDB(InfluxDbUri, InfluxDbDatabase);
                               })
                           .UseStartup<Startup>()
                           .Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }
    }
}