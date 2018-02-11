using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SemaphoreDemo
{
    internal static class Program
    {
        private static Timer _timer;
        private static readonly Semaphore Semaphore = new Semaphore(0, int.MaxValue);
        private static readonly ManualResetEvent MKillThread = new ManualResetEvent(false);
        private static readonly WaitHandle[] MWaitHandles = {Semaphore, MKillThread};

        private static void Main()
        {
            _timer = new Timer(5000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
            while (true)
            {
                WaitHandle.WaitAny(MWaitHandles);
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()}");
            }
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Semaphore.Release(3);
        }
    }
}