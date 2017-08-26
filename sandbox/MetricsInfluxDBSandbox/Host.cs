// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Infrastructure;
using App.Metrics.Reporting;
using App.Metrics.Reporting.InfluxDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace MetricsInfluxDBSandbox
{
    public static class Host
    {
        private static readonly Random Rnd = new Random();
        private static readonly string InfluxDbDatabase = "metricsinfluxdbsandboxconsole";
        private static readonly Uri InfluxDbUri = new Uri("http://127.0.0.1:8086");

        public static IServiceCollection ServiceCollection { get; } = new ServiceCollection();

        // public static async Task Main(string[] args)
        public static void Main(string[] args)
        {
            Init();

            ConfigureServices(ServiceCollection);

            var provider = ServiceCollection.BuildServiceProvider();
            var metrics = provider.GetRequiredService<IMetrics>();
            var metricsProvider = provider.GetRequiredService<IProvideMetricValues>();
            var metricsOptionsAccessor = provider.GetRequiredService<IOptions<MetricsOptions>>();
            var influxReportingOptionsAccessor = provider.GetRequiredService<IOptions<MetricsReportingInfluxDBOptions>>();
            var metricsReporter = provider.GetRequiredService<IMetricsReporter>();

            var cancellationTokenSource = new CancellationTokenSource();

            RunUntilEsc(
                TimeSpan.FromSeconds(5),
                cancellationTokenSource,
                () =>
                {
                    Console.Clear();

                    RecordMetrics(metrics);

                    Task.WaitAll(WriteMetricsAsync(
                        metricsProvider,
                        metricsReporter,
                        metricsOptionsAccessor,
                        influxReportingOptionsAccessor,
                        cancellationTokenSource).ToArray());

                    Console.WriteLine("Complete. Waiting for next run...");
                });
        }

        private static IEnumerable<Task> WriteMetricsAsync(
            IProvideMetricValues metricsProvider,
            IMetricsReporter metricsReporter,
            IOptions<MetricsOptions> metricsOptionsAccessor,
            IOptions<MetricsReportingInfluxDBOptions> influxReportingOptionsAccessor,
            CancellationTokenSource cancellationTokenSource)
        {
            var metricsData = metricsProvider.Get();

            Console.WriteLine("Metrics Formatters");
            Console.WriteLine("-------------------------------------------");

            foreach (var formatter in metricsOptionsAccessor.Value.OutputMetricsFormatters)
            {
                Console.WriteLine($"Formatter: {formatter.GetType().FullName}");
                Console.WriteLine("-------------------------------------------");

                using (var stream = new MemoryStream())
                {
                    formatter.WriteAsync(stream, metricsData, cancellationTokenSource.Token).GetAwaiter().GetResult();

                    var result = Encoding.UTF8.GetString(stream.ToArray());

                    Console.WriteLine(result);
                }
            }

            Console.WriteLine("Default Metrics Formatter");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"Formatter: {metricsOptionsAccessor.Value.DefaultOutputMetricsFormatter}");
            Console.WriteLine("-------------------------------------------");

            using (var stream = new MemoryStream())
            {
                metricsOptionsAccessor.Value.DefaultOutputMetricsFormatter.WriteAsync(stream, metricsData, cancellationTokenSource.Token).GetAwaiter().GetResult();

                var result = Encoding.UTF8.GetString(stream.ToArray());

                Console.WriteLine(result);
            }

            Console.WriteLine("Reporting Metrics");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"Formatter: {influxReportingOptionsAccessor.Value.MetricsOutputFormatter}");
            Console.WriteLine("-------------------------------------------");

            return metricsReporter.RunReportsAsync(cancellationTokenSource.Token);
        }

        private static void RecordMetrics(IMetrics metrics)
        {
            metrics.Measure.Counter.Increment(ApplicationsMetricsRegistry.CounterOne);
            metrics.Measure.Gauge.SetValue(ApplicationsMetricsRegistry.GaugeOne, Rnd.Next(0, 100));
            metrics.Measure.Histogram.Update(ApplicationsMetricsRegistry.HistogramOne, Rnd.Next(0, 100));
            metrics.Measure.Meter.Mark(ApplicationsMetricsRegistry.MeterOne, Rnd.Next(0, 100));

            using (metrics.Measure.Timer.Time(ApplicationsMetricsRegistry.TimerOne))
            {
                Thread.Sleep(Rnd.Next(0, 100));
            }

            using (metrics.Measure.Apdex.Track(ApplicationsMetricsRegistry.ApdexOne))
            {
                Thread.Sleep(Rnd.Next(0, 100));
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            services.AddMetricsReportingCore().AddInfluxDB(InfluxDbUri, InfluxDbDatabase);
            services.AddMetricsCore()
                .AddClockType<StopwatchClock>()
                .AddInfluxDBLineProtocolFormattersCore();
        }

        private static void Init()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            builder.Build();
        }

        private static void RunUntilEsc(TimeSpan delayBetweenRun, CancellationTokenSource cancellationTokenSource, Action action)
        {
            Console.WriteLine("Press ESC to stop");

            while (true)
            {
                while (!Console.KeyAvailable)
                {
                    action();
                    Thread.Sleep(delayBetweenRun);
                }

                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        cancellationTokenSource.Cancel();
                        return;
                    }
                }
            }
        }
    }
}