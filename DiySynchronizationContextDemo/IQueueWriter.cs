using System;

namespace DiySynchronizationContextDemo
{
    public interface IQueueWriter<in T> : IDisposable
    {
        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="data"></param>
        void Enqueue(T data);
    }
}