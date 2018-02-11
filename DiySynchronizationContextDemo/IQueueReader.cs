using System;

namespace DiySynchronizationContextDemo
{
    public interface IQueueReader<out T>:IDisposable
    {
        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        T Dequeque();

        /// <summary>
        /// 释放所有的数据
        /// </summary>
        void ReleaseReader();
    }
}