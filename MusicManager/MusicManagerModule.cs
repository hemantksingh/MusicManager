using Autofac;
using Autofac.Extras.DynamicProxy2;
using MusicManager.Infrastructure;

namespace MusicManager
{
    public class MusicManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainViewModel>();
            builder.RegisterAssemblyTypes(typeof(MusicManagerModule).Assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(NLogLogger));
        }
    }
}
