using Autofac;
using Autofac.Extras.DynamicProxy2;
using NLog;

namespace MusicManager
{
    public class MusicManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MusicManagerViewModel>();
            builder.Register(context => new NLogLogger(LogManager.GetLogger("TestLogger")));
            builder.RegisterAssemblyTypes(typeof(MusicManagerModule).Assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(NLogLogger));
        }
    }
}
