using System.Security.Principal;
using System.Threading;
using Autofac;
using MusicManager.Infrastructure;
using MusicManager.UI.Wpf;

namespace MusicManager
{
    class ApplicationBootStrapper
    {
        public static IContainer BootUpTheApp()
        {
            return new ApplicationBootStrapper().Bootstrap();
        }

        /// <summary>
        /// Loads the dependant modules first before loading the main application module
        /// to enable overriding the default dependency registrations.
        /// </summary>
        private IContainer Bootstrap()
        {
            var rolesUserBelongsTo = new[] { "Admin", "Generic" };
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("HK"), 
                                                           rolesUserBelongsTo);
            var builder = new ContainerBuilder();
            builder.RegisterModule<PresentationModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<MusicManagerModule>();

            return builder.Build();
        }
    }
}
