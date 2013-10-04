using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using MusicManager.UI;

namespace MusicManager
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        ///  The single place where the composition of the object graphs for the entire application
        ///  takes place. The container resolves this on application start up.
        /// </summary>
        public static object CompositionRoot;

        private IContainer _container;

        private void App_OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            const string userMessage = "Oops!!! Something went wrong. An error has been logged to the local log file.";
            const string userMessageIfLoggingFails =
                "Oops!!! Something went wrong. Poosibly an error occured while bootstrapping the application.";
            const string logMessage = "An unhandled exception has occurred";

            try
            {
                var errorHandler = _container.Resolve<IErrorHandler>();
                errorHandler.HandleError(e.Exception, logMessage, userMessage);
            }
            catch (Exception)
            {
                var promptService = new PromptService("Music Manager");
                promptService.ShowError(userMessageIfLoggingFails);
            }

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
                throw new Exception("An error ocurred while bootstrapping the applciation.", exception);
            }
        }

        private IContainer BuildAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<MusicManagerModule>();
            builder.RegisterModule<PresentationModule>();
            builder.RegisterModule<InfrastructureModule>();

            return builder.Build();
        }
    }
}