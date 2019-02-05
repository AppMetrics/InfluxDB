// <copyright file="ILineProtocolPoint.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    internal interface ILineProtocolPoint
    {
        void Write(TextWriter textWriter, bool writeTimestamp = true);
    }
}
