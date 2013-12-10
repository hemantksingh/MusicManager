using System.Security.Principal;
using System.Threading;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using MusicManager.Infrastructure;
using MusicManager.Infrastructure.Security;

namespace MusicManager
{
    public class MusicManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainViewModel>();
            builder.Register(context => Thread.CurrentPrincipal).As<IPrincipal>();

            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(InfoLoggerAspect))
                .InterceptedBy(typeof(AdminRoleAspect));

            builder.RegisterAssemblyTypes(typeof(MusicManagerModule).Assembly)
                .Where(q => q.IsPublic)
                .As(type => type.BaseType)
                .EnableClassInterceptors()
                .InterceptedBy(typeof(InfoLoggerAspect));
        }
    }
}
