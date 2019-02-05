// <copyright file="LineProtocolPointBase.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    internal abstract class LineProtocolPointBase
    {
        public LineProtocolPointBase(string measurement, MetricTags tags, DateTime? utcTimestamp)
        {
            if (string.IsNullOrEmpty(measurement))
            {
                throw new ArgumentException("A measurement name must be specified");
            }

            if (utcTimestamp != null && utcTimestamp.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Timestamps must be specified as UTC");
            }

            Measurement = measurement;
            Tags = tags;
            UtcTimestamp = utcTimestamp;
        }

        public string Measurement { get; }

        public MetricTags Tags { get; }

        public DateTime? UtcTimestamp { get; }

        protected void WriteCommon(TextWriter textWriter)
        {
            textWriter.Write(LineProtocolSyntax.EscapeName(Measurement));

            if (Tags.Count > 0)
            {
                for (var i = 0; i < Tags.Count; i++)
                {
                    textWriter.Write(',');
                    textWriter.Write(LineProtocolSyntax.EscapeName(Tags.Keys[i]));
                    textWriter.Write('=');
                    textWriter.Write(LineProtocolSyntax.EscapeName(Tags.Values[i]));
                }
            }
        }

        protected void WriteTimestamp(TextWriter textWriter)
        {
            textWriter.Write(' ');

            if (UtcTimestamp == null)
            {
                textWriter.Write(LineProtocolSyntax.FormatTimestamp(DateTime.UtcNow));
                return;
            }

            textWriter.Write(LineProtocolSyntax.FormatTimestamp(UtcTimestamp.Value));
        }
    }
}
