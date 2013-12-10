using System;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using MusicManager.Infrastructure;
using MusicManager.Shared;
using MusicManager.UI.Wpf;

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
            try
            {
                var rolesUserBelongsTo = new[] {"Admin", "Generic"};
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("HK"), rolesUserBelongsTo);
                _container = BuildAutofacContainer();
                CompositionRoot = _container.Resolve<MainViewModel>();
            }
            catch (Exception exception)
            {
                throw new Exception("An error ocurred while bootstrapping the applciation.", exception);
            }
        }

        /// <summary>
        /// Loads the dependant modules first before loading the main application module
        /// to enable overriding the default dependency registrations.
        /// </summary>
        /// <returns></returns>
        private IContainer BuildAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<PresentationModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<MusicManagerModule>();

            return builder.Build();
        }
    }
}