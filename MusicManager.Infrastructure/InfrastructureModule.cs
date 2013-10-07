using Autofac;
using Autofac.Extras.DynamicProxy2;
using MusicManager.Shared;
using NLog;

namespace MusicManager.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(InfrastructureModule).Assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(NLogLogger));
            
            Logger logger = LogManager.GetLogger("AppLogger");

            builder.Register(context => new NLogLogger(logger)).AsSelf().As<ILogger>();
        }
    }
}
