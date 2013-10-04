using System;
using MusicManager.Shared;
using NLog;

namespace MusicManager.Infrastructure
{
    class ErrorHandler : IErrorHandler
    {
        private readonly Logger _logger;
        private readonly IPromptService _promptService;

        public ErrorHandler(Logger logger, IPromptService promptService)
        {
            _logger = logger;
            _promptService = promptService;
        }

        public void HandleError(Exception exception, string logMessage, string userMessage)
        {
            _logger.ErrorException(logMessage, exception);
            _promptService.ShowError(userMessage);
        }
    }
}
