namespace BackgroundServiceSampleWeb.BackgroundServices
{
    public class ClassSampleBackgroundService : BackgroundService
    {
        private readonly ILogger<ClassSampleBackgroundService> _logger;
        public ClassSampleBackgroundService(ILogger<ClassSampleBackgroundService> logger)
        {
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
            _logger.LogInformation($"Method is running {DateTime.Now.ToLongTimeString()}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
