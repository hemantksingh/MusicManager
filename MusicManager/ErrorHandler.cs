using System;
using MusicManager.Shared;
using MusicManager.Infrastructure;

namespace MusicManager
{
    class ErrorHandler : IErrorHandler
    {
        private readonly ILogger _logger;
        private readonly IPromptService _promptService;

        public ErrorHandler(ILogger logger, IPromptService promptService)
        {
            _logger = logger;
            _promptService = promptService;
        }

        public void HandleError(Exception exception, string logMessage, string userMessage)
        {
            _logger.LogError(logMessage, exception);

            if (!string.IsNullOrWhiteSpace(userMessage))
                _promptService.ShowError(userMessage);
        }
    }
}
