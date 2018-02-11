using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows;
using Timer = System.Timers.Timer;

namespace TestSynExceptionDemo
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly Timer _timer;
        private int _i;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            _timer = new Timer(5000);
            _timer.Elapsed += AddItem_Elapsed;
            _synchronizationContext = SynchronizationContext.Current;
            Console.WriteLine($@"-----主线程ID:{Thread.CurrentThread.ManagedThreadId}");
            File.AppendAllLines(@"d:\aa.txt", new List<string>
            {
                $"主线程{Thread.CurrentThread.ManagedThreadId}"
            });
        }

        private void AddItem_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($@"-----后台线程ID:{Thread.CurrentThread.ManagedThreadId}");
            _synchronizationContext.Send(x =>
            {
                Console.WriteLine($@"-----Send线程ID:{Thread.CurrentThread.ManagedThreadId}");
                if (_i == 2) throw new ArgumentException(nameof(_i));
                ListBox.Items.Add($"Helius{_i++}");
            }, _synchronizationContext);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox.Items.Add("Helius");
        }

        private void AddItemsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void TestExceptionBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var uiContext = SynchronizationContext.Current;
            var thread = new Thread(Run);
            thread.Start(uiContext);
        }

        private void Run(object obj)
        {
            var uiContext = obj as SynchronizationContext;
            try
            {
                uiContext?.Send(AddItemsToListBox, $"Helius{_i++}");
            }
            catch (Exception)
            {
                File.AppendAllLines(@"d:\a.txt", new List<string>
                {
                    "aaaa",
                    "bbbbb"
                });
            }
        }

        private void AddItemsToListBox(object state)
        {
            throw new Exception("Boom");
        }
    }
}