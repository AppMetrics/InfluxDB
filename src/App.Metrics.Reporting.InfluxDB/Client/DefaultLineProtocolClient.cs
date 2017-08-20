// <copyright file="DefaultLineProtocolClient.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Reporting.InfluxDB.Internal;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting.InfluxDB.Client
{
    public class DefaultLineProtocolClient : ILineProtocolClient
    {
        private static long _backOffTicks;
        private static long _failureAttempts;
        private static long _failuresBeforeBackoff;
        private static TimeSpan _backOffPeriod;

        private readonly HttpClient _httpClient;
        private readonly InfluxDBOptions _influxDbOptions;
        private readonly ILogger<DefaultLineProtocolClient> _logger;

        public DefaultLineProtocolClient(
            ILogger<DefaultLineProtocolClient> logger,
            InfluxDBOptions influxDbOptions,
            HttpPolicy httpPolicy,
            HttpClient httpClient)
        {
            _influxDbOptions = influxDbOptions ?? throw new ArgumentNullException(nameof(influxDbOptions));
            _httpClient = httpClient;
            _backOffPeriod = httpPolicy?.BackoffPeriod ?? throw new ArgumentNullException(nameof(httpPolicy));
            _failuresBeforeBackoff = httpPolicy.FailuresBeforeBackoff;
            _failureAttempts = 0;
            _logger = logger;
        }

        public async Task<LineProtocolWriteResult> WriteAsync(
            string payload,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                return new LineProtocolWriteResult(true);
            }

            if (NeedToBackoff())
            {
                return new LineProtocolWriteResult(false, "Too many failures in writing to InfluxDB, Circuit Opened");
            }

            try
            {
                var content = new StringContent(payload, Encoding.UTF8);

                var response = await _httpClient.PostAsync(_influxDbOptions.Endpoint, content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    Interlocked.Increment(ref _failureAttempts);

                    var errorMessage = $"Failed to write to InfluxDB - StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase}";
                    _logger.LogError(LoggingEvents.InfluxDbWriteError, errorMessage);

                    return new LineProtocolWriteResult(false, errorMessage);
                }

                _logger.LogTrace("Successful write to InfluxDB");

                return new LineProtocolWriteResult(true);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failureAttempts);
                _logger.LogError(LoggingEvents.InfluxDbWriteError, ex, "Failed to write to InfluxDB");
                return new LineProtocolWriteResult(false, ex.ToString());
            }
        }

        private bool NeedToBackoff()
        {
            if (Interlocked.Read(ref _failureAttempts) < _failuresBeforeBackoff)
            {
                return false;
            }

            _logger.LogError($"InfluxDB write backoff for {_backOffPeriod.Seconds} secs");

            if (Interlocked.Read(ref _backOffTicks) == 0)
            {
                Interlocked.Exchange(ref _backOffTicks, DateTime.UtcNow.Add(_backOffPeriod).Ticks);
            }

            if (DateTime.UtcNow.Ticks <= Interlocked.Read(ref _backOffTicks))
            {
                return true;
            }

            Interlocked.Exchange(ref _failureAttempts, 0);
            Interlocked.Exchange(ref _backOffTicks, 0);

            return false;
        }
    }
}
