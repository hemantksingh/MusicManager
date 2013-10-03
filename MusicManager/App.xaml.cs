using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using NLog;

namespace MusicManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The single place where the composition of the object graphs for the entire application
        /// takes place. The container resolves this on application start up.
        /// </summary>
        public static object CompositionRoot;

        private void App_OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            const string userMessage = "Oops!!! Something went wrong. An error has been logged to the local log file.";
            HandleException(e.Exception, "An unhandled exception has occurred", userMessage);
            
            e.Handled = true;
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                IContainer container = BuildAutofacContainer();
                CompositionRoot = container.Resolve<MainViewModel>();
            }
            catch (Exception exception)
            {
                throw new Exception("An error ocurred while bootstrapping the applciation", exception);
            }
        }

        private IContainer BuildAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<MusicManagerModule>();

            return builder.Build();
        }

        private static void HandleException(Exception exception, string errorMessage, string userMessage)
        {
            Logger logger = LogManager.GetLogger("TestLogger");
            logger.ErrorException(errorMessage, exception);

            if(!string.IsNullOrWhiteSpace(userMessage))
                MessageBox.Show(userMessage, "Music Manager",MessageBoxButton.OK, MessageBoxImage.Stop);
        }
    }
}