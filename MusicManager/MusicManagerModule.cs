using System.Security.Principal;
using System.Threading;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using MusicManager.Infrastructure;
using MusicManager.UI.Wpf;

namespace MusicManager
{
    public class MusicManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(q => q.IsPublic)
                .As(type => type.BaseType)
                .EnableClassInterceptors()
                .InterceptedBy(typeof(InfoLoggerAspect));
            
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<MainWindow>().As<IMainView>();
            builder.Register(context => Thread.CurrentPrincipal).As<IPrincipal>()
                .SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>()
                .SingleInstance()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(InfoLoggerAspect));
        }
    }
}
