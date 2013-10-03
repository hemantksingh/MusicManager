using Autofac;

namespace MusicManager
{
    public class MusicManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MusicManagerViewModel>();
            builder.RegisterAssemblyTypes(typeof(MusicManagerModule).Assembly)
                .AsImplementedInterfaces();
        }
    }
}
