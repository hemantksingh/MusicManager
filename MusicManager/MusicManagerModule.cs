using System.Security.Principal;
using System.Threading;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using MusicManager.Infrastructure;
using MusicManager.Infrastructure.Security;
using MusicManager.UI.Wpf;

namespace MusicManager
{
    public class MusicManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(InfoLoggerAspect))
                .InterceptedBy(typeof(AdminRoleAspect));
            
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(q => q.IsPublic)
                .As(type => type.BaseType)
                .EnableClassInterceptors()
                .InterceptedBy(typeof(InfoLoggerAspect));
            
            builder.RegisterType<MainViewModel>();
            builder.Register(context => Thread.CurrentPrincipal).As<IPrincipal>();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>()
                .SingleInstance()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(InfoLoggerAspect));
        }
    }
}
