// <copyright file="MetricSnapshotInfluxDbLineProtocolWriterBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using App.Metrics.Formatters.InfluxDB;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace App.Metrics.InfluxDB.Benchmarks.BenchmarkDotNetBenchmarks.Formatters
{
    /// <summary>
    /// This benchmark examines the way <see cref="MetricSnapshotInfluxDbLineProtocolWriter"/> allocates written values.
    /// </summary>
    [Config(typeof(Config.DefaultConfig))]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class MetricSnapshotInfluxDbLineProtocolWriterBenchmark
    {
        private WritePointData[] pointData;
        private WritePointsData[] pointsData;

        [GlobalSetup]
        public void Setup()
        {
            var tags = new MetricTags(new[] { "app", "host" }, new[] { "my_app", "my_host" });

            // generate 500x points
            pointData = new WritePointData[500];
            for (int i = 0; i < 500; i++)
            {
                pointData[i] = new WritePointData { Context = "context", Name = "name", Field = "field", Tags = tags, Value = 50d, Timestamp = DateTime.UtcNow };
            }

            pointsData = new WritePointsData[500];
            for (int i = 0; i < 500; i++)
            {
                pointsData[i] = new WritePointsData { Context = "context", Name = "name", Tags = tags, Columns = new[] { "col_1", "col_2" }, Values = new object[] { 50d, 45d }, Timestamp = DateTime.UtcNow };
            }
        }

        [BenchmarkCategory("SingleValue")]
        [Benchmark(Baseline = true)]
        [Obsolete("Legacy baseline benchs")]
        public void WriteSingleValuePoint_Legacy()
        {
            var writer = new MetricSnapshotInfluxDbLineProtocolWriter(TextWriter.Null);
            var p = pointData[0];
            writer.WriteLegacy(p.Context, p.Name, p.Field, p.Value, p.Tags, p.Timestamp);
        }

        [BenchmarkCategory("SingleValue")]
        [Benchmark]
        public void WriteSingleValuePoint()
        {
            var writer = new MetricSnapshotInfluxDbLineProtocolWriter(TextWriter.Null);
            var p = pointData[0];
            writer.Write(p.Context, p.Name, p.Field, p.Value, p.Tags, p.Timestamp);
        }

        [BenchmarkCategory("MultipleValues")]
        [Benchmark(Baseline = true)]
        [Obsolete("Legacy baseline benchs")]
        public void WriteMultipleValuesPoint_Legacy()
        {
            var writer = new MetricSnapshotInfluxDbLineProtocolWriter(TextWriter.Null);
            var p = pointsData[0];
            writer.WriteLegacy(p.Context, p.Name, p.Columns, p.Values, p.Tags, p.Timestamp);
        }

        [BenchmarkCategory("MultipleValues")]
        [Benchmark]
        public void WriteMultipleValuesPoint()
        {
            var writer = new MetricSnapshotInfluxDbLineProtocolWriter(TextWriter.Null);
            var p = pointsData[0];
            writer.Write(p.Context, p.Name, p.Columns, p.Values, p.Tags, p.Timestamp);
        }

        private class WritePointData
        {
            public string Context { get; set; }

            public string Name { get; set; }

            public string Field { get; set; }

            public object Value { get; set; }

            public MetricTags Tags { get; set; }

            public DateTime Timestamp { get; set; }
        }

        private class WritePointsData
        {
            public string Context { get; set; }

            public string Name { get; set; }

            public IEnumerable<string> Columns { get; set; }

            public IEnumerable<object> Values { get; set; }

            public MetricTags Tags { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }
}
