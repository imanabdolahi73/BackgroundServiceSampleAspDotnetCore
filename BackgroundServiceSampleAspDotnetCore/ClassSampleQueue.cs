using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BackgroundServiceSampleAspDotnetCore
{
    public class ClassSampleQueue
    {
        public Channel<string> _channel;
        public ClassSampleQueue()
        {
            _channel = Channel.CreateUnbounded<string>();
        }
    }
    public class ClassSampleProducer
    {
        private Channel<string> _channel;
        public ClassSampleProducer(Channel<string> channel)
        {
            _channel = channel;
        }

        public Task SendMessage()
        {
            return Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(1000);
                    await _channel.Writer.WriteAsync($"Message {i}");
                }
            });
        }
    }
    public class ClassSampleCunsomer
    {
        private Channel<string> _channel;
        public ClassSampleCunsomer(Channel<string> channel)
        {
            _channel = channel;
        }

        public Task GetMessage()
        {
            return Task.Run(async () =>
            {
                while(await _channel.Reader.WaitToReadAsync())
                {
                    string message = await _channel.Reader.ReadAsync();
                    Console.WriteLine(message);
                }
            });
        }
    }
}
