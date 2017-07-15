// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using App.Metrics.Builder;
using App.Metrics.Core.Filtering;
using App.Metrics.InfluxDB.Sandbox.JustForTesting;
using App.Metrics.Reporting.InfluxDB;
using App.Metrics.Reporting.InfluxDB.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.InfluxDB.Sandbox
{
    public class Startup
    {
        private static readonly bool HaveAppRunSampleRequests = true;
        private static readonly string InfluxDbDatabase = "appmetricssandbox";
        private static readonly Uri InfluxDbUri = new Uri("http://127.0.0.1:8086");
        private static readonly bool RunSamplesWithClientId = true;

        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public static IWebHost BuildSandboxWebHost(string[] args)
        {
            return new WebHostBuilder().UseContentRoot(Directory.GetCurrentDirectory()).
                                        ConfigureAppConfiguration(
                                            (context, builder) =>
                                            {
                                                builder.SetBasePath(context.HostingEnvironment.ContentRootPath).
                                                        AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
                                                        AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true).
                                                        AddEnvironmentVariables();
                                            }).
                                        ConfigureLogging(
                                            factory =>
                                            {
                                                factory.AddConsole();
                                            }).
                                        UseIISIntegration().
                                        UseKestrel().
                                        UseStartup<Startup>().
                                        Build();
        }

        public static void Main(string[] args) { BuildSandboxWebHost(args).Run(); }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            if (RunSamplesWithClientId && HaveAppRunSampleRequests)
            {
                app.Use(
                    (context, func) =>
                    {
                        RandomClientIdForTesting.SetTheFakeClaimsPrincipal(context);
                        return func();
                    });
            }

            app.UseMetrics();
            app.UseMetricsReporting(lifetime);

            app.UseMvc();

            if (HaveAppRunSampleRequests)
            {
                SampleRequests.Run(lifetime.ApplicationStopping);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTestStuff();
            services.AddLogging().AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            services.AddMetrics(Configuration.GetSection("AppMetrics")).
                     AddReporting(
                         factory =>
                         {
                             factory.AddInfluxDb(
                                 new InfluxDBReporterSettings
                                 {
                                     InfluxDbSettings = new InfluxDBSettings(InfluxDbDatabase, InfluxDbUri),
                                     ReportInterval = TimeSpan.FromSeconds(5)
                                 });
                         }).
                         AddMetricsMiddleware(
                         Configuration.GetSection("AspNetMetrics"),
                         optionsBuilder =>
                         {
                             optionsBuilder.AddMetricsInfluxDBLineProtocolFormatters().
                                            AddMetricsTextAsciiFormatters().
                                            AddEnvironmentAsciiFormatters();
                         });

            services.
                AddHealthChecks().
                AddHealthCheckMiddleware(optionsBuilder => optionsBuilder.AddAsciiFormatters()).
                AddChecks(registry =>
                {
                    registry.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                    registry.AddHttpGetCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));
                });
        }
    }
}