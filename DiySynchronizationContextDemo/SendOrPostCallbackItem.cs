using System;
using System.Threading;

namespace DiySynchronizationContextDemo
{
    public class SendOrPostCallbackItem
    {
        private readonly object _state;
        private readonly ExecutionType _executionType;
        private readonly SendOrPostCallback _method;
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private Exception _exception;

        internal SendOrPostCallbackItem(SendOrPostCallback callback, object state, ExecutionType executionType)
        {
            _method = callback;
            _state = state;
            _executionType = executionType;
        }

        internal Exception Exception => _exception;

        internal bool ExecuteWithException => _exception != null;

        internal WaitHandle ExecutionCompleteWaitHandle => _manualResetEvent;

        internal void Execute()
        {
            if (_executionType == ExecutionType.Send)
            {
                Send();
            }
            else
            {
                Post();
            }
        }

        private void Post()
        {
            _method(_state);
        }

        private void Send()
        {
            try
            {
                _method(_state);
            }
            catch (Exception e)
            {
                _exception = e;
            }
            finally
            {
                _manualResetEvent.Set();
            }
        }
    }

    enum ExecutionType
    {
        Post,
        Send
    }
}