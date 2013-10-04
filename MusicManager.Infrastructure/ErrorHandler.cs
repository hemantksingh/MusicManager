using System;
using NLog;

namespace MusicManager.Infrastructure
{
    class ErrorHandler : IErrorHandler
    {
        private readonly Logger _logger;

        public ErrorHandler(Logger logger)
        {
            _logger = logger;
        }

        public void HandleError(Exception exception, string logMessage, string userMessage)
        {
            _logger.ErrorException(logMessage, exception);
        }
    }
}
