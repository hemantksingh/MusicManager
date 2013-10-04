using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using MusicManager.Infrastructure;

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

        private IContainer _container;

        private void App_OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            const string userMessage = "Oops!!! Something went wrong. An error has been logged to the local log file.";
            const string logMessage = "An unhandled exception has occurred";

            var errorHandler = _container.Resolve<IErrorHandler>();
            errorHandler.HandleError(e.Exception, logMessage, userMessage);
            
            e.Handled = true;
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                _container = BuildAutofacContainer();
                CompositionRoot = _container.Resolve<MainViewModel>();
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
            builder.RegisterModule<InfrastructureModule>();

            return builder.Build();
        }
    }
}