
using Autofac;
using Autofac.Extras.DynamicProxy2;
using MusicManager.Infrastructure;
using MusicManager.Infrastructure.Security;
using MusicManager.Shared;

namespace MusicManager.UI
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(PresentationModule).Assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(InfoLoggerAspect));

            builder.Register(context => new PromptService("Music Manager"))
                   .As<IPromptService>()
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(InfoLoggerAspect))
                   .InterceptedBy(typeof(AdminRoleAspect));
        }
    }
}
