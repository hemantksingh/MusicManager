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
            const string applicationTitle = "Music Manager";
            const string logMessage = "An unhandled exception occurred";

            string userMessage = string.Format("Oops!!! '{0}' has ran into a problem ." 
                                    + "An error has been logged to the local log file.", 
                                    applicationTitle);
            string userMsgIfErrorHandlingFails = string.Format("Oops!!! '{0}' has ran into a problem ." 
                                    + "Poosibly an error occured while bootstrapping the application.", 
                                    applicationTitle);
            try
            {
                var errorHandler = _container.Resolve<IErrorHandler>();
                errorHandler.HandleError(e.Exception, logMessage, userMessage);
            }
            catch (Exception exception)
            {
                var logger = _container.Resolve<ILogger>();
                logger.LogError(logMessage, exception);

                var promptService = new PromptService(applicationTitle);
                promptService.ShowError(userMsgIfErrorHandlingFails);
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