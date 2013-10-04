using System;

namespace MusicManager.Shared
{
    public interface IErrorHandler
    {
        void HandleError(Exception exception, string logMessage, string userMessage);
    }
}