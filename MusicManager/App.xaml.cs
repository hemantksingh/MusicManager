using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using MusicManager.UI.Wpf;

namespace MusicManager
{
    /// <summary>
    ///  Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;

        private void App_OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            const string applicationTitle = "Music Manager";
            const string logMessage = "An unhandled exception occurred";

            string userMessage = string.Format("Oops!!! '{0}' ran into a problem. " 
                                    + "The error details have been saved to the local log file.", 
                                    applicationTitle);
            string userMsgIfErrorHandlingFails = string.Format("Oops!!! '{0}' ran into a problem. " 
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
            _container = ApplicationBootStrapper.BootUpTheApp();
            var mainView = _container.Resolve<IMainView>();
            mainView.Show();
        }
    }
}