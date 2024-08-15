using System.Globalization;

namespace BackgroundServiceSampleWeb.BackgroundServices
{
    public class ClassSampleIHostedService : IHostedService , IDisposable
    {
        private Timer _timer = null;
        private int execCount = 0;
        private readonly ILogger<ClassSampleIHostedService> _logger;
        public ClassSampleIHostedService(ILogger<ClassSampleIHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Task is running.");

            //_timer = new Timer(TestMethod, null, TimeSpan.FromSeconds(1) , TimeSpan.FromSeconds(1));

            //every day 
            _timer = new Timer(TestMethod, null, GetJobRunDelay(), TimeSpan.FromHours(24));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Task is stopping.");
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }


        private void TestMethod(object? state)
        {
            //When This Methis Call With Several Thred
            var count = Interlocked.Increment(ref execCount);
            _logger.LogWarning($"Test Method Started ... {count}");
            Thread.Sleep(5000);
            _logger.LogInformation($"Test Method Ended ... {count}");
        }


        private TimeSpan GetScheduledPersedTime()
        {
            string[] formats = { @"hh\:mm\:ss", "hh\\:mm" };
            string jobStartTime = "14:31";
            TimeSpan.TryParseExact(jobStartTime, formats, CultureInfo.InvariantCulture, out TimeSpan ScheduledTimeSpan);
            return ScheduledTimeSpan;
        }

        private TimeSpan GetJobRunDelay()
        {
            TimeSpan scheduledParsedTime = GetScheduledPersedTime();
            TimeSpan currentTimeOftheDay = TimeSpan.Parse(DateTime.Now.TimeOfDay.ToString("hh\\:mm"));
            TimeSpan delayTime = scheduledParsedTime >= currentTimeOftheDay
                ? scheduledParsedTime - currentTimeOftheDay
                : new TimeSpan(24, 0, 0) - currentTimeOftheDay + scheduledParsedTime;
            return delayTime;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
