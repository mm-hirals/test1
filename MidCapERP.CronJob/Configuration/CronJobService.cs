﻿using Cronos;
using Microsoft.Extensions.Hosting;

namespace MidCapERP.CronJob
{
    public class CronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;

        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo)
        {
            _expression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;

                // interval can be int.MaxValue at max
                if (Math.Ceiling(delay.TotalMilliseconds) > int.MaxValue)
                {
                    // set timer to int.MaxValue
                    _timer = new System.Timers.Timer(int.MaxValue);
                    _timer.Elapsed += async (sender, args) =>
                    {
                        _timer.Dispose();  // reset and dispose timer
                        _timer = null;

                        if (!cancellationToken.IsCancellationRequested)
                        {
                            await ScheduleJob(cancellationToken);    // reschedule next
                        }
                    };
                    _timer.Start();
                }
                else
                {
                    _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                    _timer.Elapsed += async (sender, args) =>
                    {
                        _timer.Dispose();  // reset and dispose timer
                        _timer = null;

                        if (!cancellationToken.IsCancellationRequested)
                        {
                            await DoWork(cancellationToken);
                        }

                        if (!cancellationToken.IsCancellationRequested)
                        {
                            await ScheduleJob(cancellationToken);    // reschedule next
                        }
                    };
                    _timer.Start();
                }
            }
            await Task.CompletedTask;
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }
    }
}