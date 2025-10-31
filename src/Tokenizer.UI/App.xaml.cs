using System;
using System.Windows;

namespace Tokenizer.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, args) =>
            {
                MessageBox.Show(args.ExceptionObject?.ToString() ?? "Unhandled exception");
            };
            this.DispatcherUnhandledException += (s, args) =>
            {
                MessageBox.Show(args.Exception.ToString(), "Unhandled UI exception");
                args.Handled = true;
            };

            base.OnStartup(e);

            var wnd = new Views.MainWindow();
            wnd.Show();
        }
    }
}