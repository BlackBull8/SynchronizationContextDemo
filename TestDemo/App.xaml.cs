using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace TestSynExceptionDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            File.AppendAllLines(@"d:\aa.txt",new List<string>
            {
                "ccc",
                "dddd",
                $"{Thread.CurrentThread.ManagedThreadId}"
            });
            e.Handled = true;
        }
    }
}
