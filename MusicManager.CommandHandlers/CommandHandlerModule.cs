using Autofac;

namespace MusicManager.CommandHandlers
{
    public class CommandHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().Assembly).AsSelf();
        }
    }
}
