using System.Security.Principal;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using MusicManager.Infrastructure.Security;
using NLog;

namespace MusicManager.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                   .AsImplementedInterfaces()
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof (InfoLoggerAspect));

            builder.Register(context => LogManager.GetLogger("AppLogger")).As<Logger>();
            builder.Register(context => new InfoLoggerAspect(context.Resolve<Logger>())).AsSelf().As<ILogger>();
            builder.Register(context => new AdminRoleAspect(context.Resolve<Logger>(),
                                                              context.Resolve<IPrincipal>()));
        }
    }
}