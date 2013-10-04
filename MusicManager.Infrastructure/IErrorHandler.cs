using System;

namespace MusicManager.Infrastructure
{
    public interface IErrorHandler
    {
        void HandleError(Exception exception, string logMessage, string userMessage);
    }
}