﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestDemoConsole
{
    static class Program
    {
        private static SynchronizationContext _mT1 = null;
        static void Main(string[] args)
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Main thread is " + id);

            // create a sync context for this thread
            var context = new SynchronizationContext();
            // set this context for this thread.
            SynchronizationContext.SetSynchronizationContext(context);

            // create a thread, and pass it the main sync context.
            Thread t1 = new Thread(new ParameterizedThreadStart(Run1));
            t1.Start(SynchronizationContext.Current);
            Console.ReadLine();
        }

        private static void Run1(object state)
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Run1 Thread ID: " + id);

            // grab  the sync context that main has set
            var context = state as SynchronizationContext;

            // call the sync context of main, expecting
            // the following code to run on the main thread
            // but it will not.
            context.Send(DoWork, null);

            while (true)
                Thread.Sleep(10000000);
        }

        static void DoWork(object state)
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("DoWork Thread ID:" + id);
        }
    }
}
