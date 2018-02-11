using System;
using System.Threading;

namespace DiySynchronizationContextDemo
{
    internal static class Program
    {
        private static int _count;
        private static StaSynchronizationContext _staSynchronizationContext;

        private static void Main()
        {
            _staSynchronizationContext = new StaSynchronizationContext();
            for (var i = 0; i < 100; i++)
                ThreadPool.QueueUserWorkItem(NonStaThread);

            Console.WriteLine("Processing");
            Console.WriteLine("Press any key to dispose SyncContext");
            Console.ReadLine();
            _staSynchronizationContext.Dispose();
        }

        private static void NonStaThread(object state)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            for (var i = 0; i < 10; i++)
            {
                var param = new Params {OriginalThread = id, CallCounter = i};
                _staSynchronizationContext.Send(RunOnStaThread, param);
            }
        }

        private static void RunOnStaThread(object state)
        {
            _count++;
            Console.WriteLine(_count);

            var id = Thread.CurrentThread.ManagedThreadId;

            var args = (Params) state;
            Console.WriteLine($"STA id {id} original thread {args.OriginalThread} call count {args.CallCounter}");

            args.Output = "Processed";
        }
    }

    public class Params
    {
        public string Output { get; set; }
        public int CallCounter { get; set; }
        public int OriginalThread { get; set; }
    }
}