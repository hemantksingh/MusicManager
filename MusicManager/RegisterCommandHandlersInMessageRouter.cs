using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using MusicManager.CommandHandlers;
using MusicManager.Infrastructure.Bus;

namespace MusicManager
{
    internal class RegisterCommandHandlersInMessageRouter
    {
        private static MethodInfo _createPublishActionWrappedInTransactionMethod;
        private static MethodInfo _registerMethod;

        public static void BootStrap(IContainer container)
        {
            var messageRouter = container.Resolve<IMessageRouter>() as MessageRouter;
            new RegisterCommandHandlersInMessageRouter().RegisterRoutes(messageRouter, container);
        }

        private void RegisterRoutes(MessageRouter messageRouter, IContainer container)
        {
            _createPublishActionWrappedInTransactionMethod =
                GetType().GetMethod("CreatePublishActionWrappedInTransaction");
            _registerMethod = messageRouter.GetType().GetMethod("Register");

            IEnumerable<Type> commands = CommandReader.GetCommands();
            IDictionary<Type, IList<Type>> commandHandlers = CommandReader.GetCommandHandlers();

            foreach (Type command in commands)
            {
                IList<Type> commandHandlerTypes;
                if (!commandHandlers.TryGetValue(command, out commandHandlerTypes))
                    throw new Exception(string.Format("No command handlers found for event '{0}'", command.FullName));

                foreach (Type commandHandler in commandHandlerTypes)
                {
                    object injectedCommandHandler = container.Resolve(commandHandler);
                    object action = CreateTheProperAction(command, injectedCommandHandler);
                    RegisterTheCreatedActionWithTheMessageRouter(messageRouter, command, action);
                }
            }
        }

        private static void RegisterTheCreatedActionWithTheMessageRouter(MessageRouter messageRouter, Type commandType,
                                                                         object action)
        {
            _registerMethod.MakeGenericMethod(commandType).Invoke(messageRouter, new[] {action});
        }

        private object CreateTheProperAction(Type commandType, object commandHandler)
        {
            return
                _createPublishActionWrappedInTransactionMethod.MakeGenericMethod(commandType, commandHandler.GetType())
                                                              .Invoke(this, new[] {commandHandler});
        }

        public Action<TCommand> CreatePublishActionWrappedInTransaction<TCommand, TCommandHandler>(
            TCommandHandler commandHandler)
            where TCommand : class
            where TCommandHandler : ICommandHandler<TCommand>
        {
            return commandHandler.Execute;
        }
    }
}