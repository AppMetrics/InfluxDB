// <copyright file="Program.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using App.Metrics.InfluxDB.Benchmarks.Support;
using BenchmarkDotNet.Running;

namespace App.Metrics.InfluxDB.Benchmarks.Runner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleKeyInfo keyInfo;

            do
            {
                BenchmarkSwitcher.FromAssembly(typeof(BenchmarksAssemblyMarker).GetTypeInfo().Assembly).Run(args);

                Console.WriteLine("Press ESC to quit, otherwise any key to continue...");

                keyInfo = Console.ReadKey(true);
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }
    }
}
