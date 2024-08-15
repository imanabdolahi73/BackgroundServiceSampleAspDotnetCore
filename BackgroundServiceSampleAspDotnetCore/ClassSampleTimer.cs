using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundServiceSampleAspDotnetCore
{
    public class ClassSampleTimer
    {
        private int invokeCount;
        private int maxCount;
        public ClassSampleTimer(int count)
        {
            maxCount = count;
        }

        public void TestMethod(object stateInfo)
        {
            AutoResetEvent autoReset = (AutoResetEvent)stateInfo;

            Console.WriteLine($"Test Method ... {++invokeCount}");

            if(invokeCount == maxCount)
            {
                invokeCount = 0;
                autoReset.Set();
            }
        }
    }
}
