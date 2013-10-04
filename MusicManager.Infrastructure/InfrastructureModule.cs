using Autofac;
using NLog;

namespace MusicManager.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Logger logger = LogManager.GetLogger("TestLogger");

            builder.Register(context => new ErrorHandler(logger)).As<IErrorHandler>();
            builder.Register(context => new NLogLogger(logger));
        }
    }
}
