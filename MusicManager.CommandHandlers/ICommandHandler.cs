﻿namespace MusicManager.CommandHandlers
{
    public interface ICommandHandler<in TCommand> where TCommand: class
    {
        void Execute(TCommand command);
    }
}