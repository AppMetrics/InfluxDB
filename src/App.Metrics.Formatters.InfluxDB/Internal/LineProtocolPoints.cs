// <copyright file="LineProtocolPoints.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    internal class LineProtocolPoints
    {
        private readonly List<LineProtocolPoint> _points = new List<LineProtocolPoint>();

        public void Add(LineProtocolPoint point)
        {
            if (point == null)
            {
                return;
            }

            _points.Add(point);
        }

        public void Write(TextWriter textWriter, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                return;
            }

            var points = _points.ToList();

            foreach (var point in points)
            {
                point.Write(textWriter, writeTimestamp);
                textWriter.Write('\n');
            }
        }
    }
}
