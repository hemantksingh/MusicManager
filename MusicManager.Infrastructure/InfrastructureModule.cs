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
            
            Logger logger = LogManager.GetLogger("TestLogger");

            builder.Register(context => new ErrorHandler(logger, context.Resolve<IPromptService>())).As<IErrorHandler>();
            builder.Register(context => new NLogLogger(logger));
        }
    }
}
