using System;

namespace MusicManager.Infrastructure
{
    /// <summary>
    /// An infrastructure specific logger abstracting the logging behaviour and implementation.
    /// </summary>
    public interface ILogger
    {
        void LogError(string message, Exception exception);
    }
}