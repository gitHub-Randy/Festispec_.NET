using FestiSpec.Entities.Dal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;
using FestiSpec.Views;
using System.Threading;
using System.Windows.Threading;

namespace FestiSpec
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += new
    UnhandledExceptionEventHandler(AppDomainUnhandledExceptionHandler);

            System.Windows.Forms.Application.ThreadException += new
    ThreadExceptionEventHandler(Application_ThreadException);

            Application.Current.DispatcherUnhandledException += new
    DispatcherUnhandledExceptionEventHandler(AppDispatcherUnhandledException);
        }

        void OnShutdown(object sender, ExitEventArgs e)
        {
            DBContext.Instance.SaveChanges();
            DBContext.Instance.Close();
        }

        private bool _done = false;
        void UncaughtException(object sender, Exception e)
        {
            if (_done) return;
            _done = true;

            Console.WriteLine("MyHandler caught : " + e.Message);

            MessageBox.Show("Je bent de verbinding verloren.", "Fout");

            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }

            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        void AppDomainUnhandledExceptionHandler(object sender,
        UnhandledExceptionEventArgs ea)
        {
            UncaughtException(sender, (Exception)ea.ExceptionObject);
        }

        void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            UncaughtException(sender, (Exception)e.Exception);
        }

        void AppDispatcherUnhandledException(object
        sender, DispatcherUnhandledExceptionEventArgs e)
        {
            UncaughtException(sender, (Exception)e.Exception);
        }
    }
}
