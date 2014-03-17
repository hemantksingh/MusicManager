using System.Security.Principal;
using System.Threading;
using Autofac;

namespace MusicManager
{
    public class ApplicationBootStrapper
    {
        public static IContainer BootUpTheApp(IContainerFactory containerFactory)
        {
            return new ApplicationBootStrapper().Bootstrap(containerFactory);
        }

        /// <summary>
        /// Loads the dependant modules first before loading the main application module
        /// to enable overriding the default dependency registrations.
        /// </summary>
        private IContainer Bootstrap(IContainerFactory containerFactory)
        {
            var rolesUserBelongsTo = new[] { "Admin", "Generic" };
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("HK"), 
                                                           rolesUserBelongsTo);

            IContainer container = containerFactory.CreateContainer();
            RegisterCommandHandlersInMessageRouter.BootStrap(container);
            return container;
        }
    }
}
