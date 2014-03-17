using Autofac;

namespace MusicManager
{
    public interface IContainerFactory
    {
        IContainer CreateContainer();
        void OverrideDefaultRegistrations();
    }
}