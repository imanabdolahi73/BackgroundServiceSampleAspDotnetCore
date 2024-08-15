using System.Threading.Channels;

namespace BackgroundServiceSampleWeb.Queue
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueBackgroundworkItemAsync(Func<CancellationToken, ValueTask> workItem);
        ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
    }

    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {

        private readonly Channel<Func<CancellationToken, ValueTask>> _queue;
        public BackgroundTaskQueue(int capacity)
        {
            BoundedChannelOptions options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait,//if capacity is full , queue waited for fininsh a task and add to queue
            };
            _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
        }

        public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);
            return workItem;
        }

        public async ValueTask QueueBackgroundworkItemAsync(Func<CancellationToken, ValueTask> workItem)
        {
            if (workItem is null)
                throw new ArgumentException(nameof(workItem));

            await _queue.Writer.WriteAsync(workItem);

        }
    }

    public class SamplePruducer
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<SamplePruducer> _logger;
        public SamplePruducer(IBackgroundTaskQueue taskQueue ,ILogger<SamplePruducer> logger)
        {
            _taskQueue = taskQueue;
            _logger = logger;
        }

        public void SampleMethodBackground()
        {
            _taskQueue.QueueBackgroundworkItemAsync(BuildWorkItem);
        }

        private async ValueTask BuildWorkItem(CancellationToken cancellationToken)
        {
            var workIsDone = false;
            while(!cancellationToken.IsCancellationRequested && workIsDone == false)
            {
                workIsDone = await SampleMainWorkThatYouNeedWorkInBackGround(cancellationToken);
            }
        }

        private async Task<bool> SampleMainWorkThatYouNeedWorkInBackGround(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(2000 , cancellationToken);
            _logger.LogInformation($"SampleMainWork is Done {DateTime.Now.ToLongTimeString()}");
            return true;
        }
    }

    public class QueueHostedService : BackgroundService
    {
        private readonly ILogger<QueueHostedService> _logger;
        public IBackgroundTaskQueue TaskQueue { get; }
        public QueueHostedService(ILogger<QueueHostedService> logger, 
            IBackgroundTaskQueue taskQueue)
        {
            _logger = logger;
            TaskQueue = taskQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Start QueueHostedService");
            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueAsync(cancellationToken);
                try
                {
                    await workItem.Invoke(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,$"Error in WorkItem {ex.Message}");
                }
            }
        }
    }
}
