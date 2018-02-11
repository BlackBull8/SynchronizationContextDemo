using System.Collections.Generic;
using System.Threading;

namespace DiySynchronizationContextDemo
{
    public class BlockingQueue<T>:IQueueReader<T>,IQueueWriter<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private Semaphore _semaphore = new Semaphore(0,int.MaxValue);
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private readonly WaitHandle[] _waitHandles;

        public BlockingQueue()
        {
            _waitHandles = new WaitHandle[] {_semaphore, _manualResetEvent};
        }

        public void Enqueue(T data)
        {
            lock (_queue)
            {
                _queue.Enqueue(data);
            }
            _semaphore.Release();
        }

        public T Dequeque()
        {
            WaitHandle.WaitAny(_waitHandles);
            lock (_queue)
            {
                if (_queue.Count > 0)
                {
                    return _queue.Dequeue();
                }
            }
            return default(T);
        }

        public void ReleaseReader()
        {
            _manualResetEvent.Set();
        }

        public void Dispose()
        {
            if (_semaphore != null)
            {
                _semaphore.Close();
                _queue.Clear();
                _semaphore = null;
            }
        }
    }
}