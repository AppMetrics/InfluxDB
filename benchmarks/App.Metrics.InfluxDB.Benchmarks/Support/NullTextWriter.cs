// <copyright file="NullTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;

namespace App.Metrics.InfluxDB.Benchmarks.Support
{
    /// <summary>
    /// This textwriter discards all values.
    /// </summary>
    public class NullTextWriter : TextWriter
    {
        public NullTextWriter()
        {
        }

        public NullTextWriter(IFormatProvider formatProvider)
            : base(formatProvider)
        {
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}
