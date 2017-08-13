// <copyright file="ILineProtocolClient.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting.InfluxDB.Client
{
    public interface ILineProtocolClient
    {
        Task<LineProtocolWriteResult> WriteAsync(
            string payload,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}