using System;
using System.Collections.Generic;
using Autofac;
using MusicManager.CommandHandlers;
using MusicManager.Infrastructure;
using MusicManager.UI.Wpf;

namespace MusicManager
{
    public class ContainerFactory : IContainerFactory
    {
        private readonly Dictionary<Type, object> _dependencies;
        private readonly ContainerBuilder _containerBuilder;

        public ContainerFactory(Dictionary<Type, object> dependencies)
        {
            _dependencies = dependencies;
            _containerBuilder = new ContainerBuilder();
        }

        public IContainer CreateContainer()
        {
            RegisterTypes();
            OverrideDefaultRegistrations();

            return BuildContainer();
        }

        private IContainer BuildContainer()
        {
            return _containerBuilder.Build();
        }

        public virtual void OverrideDefaultRegistrations()
        {
            foreach (KeyValuePair<Type, object> dependency in _dependencies)
            {
                _containerBuilder.Register(c => dependency.Value).As(dependency.Key);
            }
        }

        private void RegisterTypes()
        {
            _containerBuilder.RegisterModule<CommandHandlerModule>();
            _containerBuilder.RegisterModule<PresentationModule>();
            _containerBuilder.RegisterModule<InfrastructureModule>();
            _containerBuilder.RegisterModule<MusicManagerModule>();
        }
    }
}
