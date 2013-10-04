
using Autofac;
using MusicManager.Shared;

namespace MusicManager.UI
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new PromptService("Music Manager"))
                   .As<IPromptService>();
        }
    }
}
