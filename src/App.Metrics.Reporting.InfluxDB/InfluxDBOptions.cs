// <copyright file="InfluxDBOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Reporting.InfluxDB
{
    /// <summary>
    ///     Provides programmatic configuration for InfluxDB in the App Metrics framework.
    /// </summary>
    public class InfluxDbOptions
    {
        /// <summary>
        ///     Gets or sets the number of InfluxDB notes that must confirm the write
        /// </summary>
        /// <value>
        ///     The InfluxDB node write consistency.
        /// </value>
        public string Consistenency { get; set; }

        /// <summary>
        ///     Gets formatted endpoint for writes to InfluxDB
        /// </summary>
        /// <value>
        ///     The InfluxDB endpoint for writes.
        /// </value>
        public string Endpoint
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Database))
                {
                    return null;
                }

                var endpoint = $"write?db={Uri.EscapeDataString(Database)}";

                if (!string.IsNullOrWhiteSpace(RetensionPolicy))
                {
                    endpoint += $"&rp={Uri.EscapeDataString(RetensionPolicy)}";
                }

                if (!string.IsNullOrWhiteSpace(Consistenency))
                {
                    endpoint += $"&consistency={Uri.EscapeDataString(Consistenency)}";
                }

                return endpoint;
            }
        }

        /// <summary>
        ///     Gets or sets the base URI of the InfluxDB API.
        /// </summary>
        /// <value>
        ///     The base URI of the InfluxDB API where metrics are flushed.
        /// </value>
        public Uri BaseUri { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB database name used to report metrics.
        /// </summary>
        /// <value>
        ///     The InfluxDB database name where metrics are flushed.
        /// </value>
        public string Database { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB database password.
        /// </summary>
        /// <value>
        ///     The InfluxDB database password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB database's retention policy to target.
        /// </summary>
        /// <value>
        ///     The InfluxDB database's retention policy to target.
        /// </value>
        public string RetensionPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB database username.
        /// </summary>
        /// <value>
        ///     The InfluxDB database username.
        /// </value>
        public string UserName { get; set; }
    }
}