using System;
using System.Threading;

namespace DiySynchronizationContextDemo
{
    public class StaSynchronizationContext:SynchronizationContext,IDisposable
    {
        private readonly BlockingQueue<SendOrPostCallbackItem> _queue;
        private readonly StaThread _staThread;

        public StaSynchronizationContext()
        {
            _queue= new BlockingQueue<SendOrPostCallbackItem>();
            _staThread = new StaThread(_queue);
            _staThread.Start();
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            SendOrPostCallbackItem item = new SendOrPostCallbackItem(d,state,ExecutionType.Send);
            _queue.Enqueue(item);

            item.ExecutionCompleteWaitHandle.WaitOne();

            if (item.ExecuteWithException)
            {
                throw item.Exception;
            }
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            SendOrPostCallbackItem item = new SendOrPostCallbackItem(d,state,ExecutionType.Post);

            _queue.Enqueue(item);
        }

        public void Dispose()
        {
            _staThread.Stop();
        }

        public override SynchronizationContext CreateCopy()
        {
            return this;
        }
    }
}