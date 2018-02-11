using System.Threading;

namespace DiySynchronizationContextDemo
{
    public class StaThread
    {
        private readonly Thread _staThread;
        private readonly IQueueReader<SendOrPostCallbackItem> _queueConsumer;
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

        internal StaThread(IQueueReader<SendOrPostCallbackItem> reader)
        {
            _queueConsumer = reader;
            _staThread = new Thread(Run) {Name = "STA Worker Thread"};
            _staThread.SetApartmentState(ApartmentState.STA);
        }

        private void Run()
        {
            while (true)
            {
                bool stop = _manualResetEvent.WaitOne(0);
                if (stop) break;

                SendOrPostCallbackItem workItem = _queueConsumer.Dequeque();
                workItem?.Execute();
            }
        }

        internal void Start()
        {
            _staThread.Start();
        }

        internal void Join()
        {
            _staThread.Join();
        }

        internal void Stop()
        {
            _manualResetEvent.Set();
            _queueConsumer.ReleaseReader();
            _staThread.Join();
            _queueConsumer.Dispose();
        }
    }
}