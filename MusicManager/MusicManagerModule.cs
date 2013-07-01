using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
