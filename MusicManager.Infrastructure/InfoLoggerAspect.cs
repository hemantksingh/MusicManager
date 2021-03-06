using System;
using System.Linq;
using Castle.DynamicProxy;
using NLog;

namespace MusicManager.Infrastructure
{
    public class InfoLoggerAspect : IInterceptor, ILogger
    {
        private readonly Logger _logger;

        public InfoLoggerAspect(Logger logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            Type targetType = invocation.TargetType;
            string methodName = invocation.Method.Name;
            string arguments = string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray());

            _logger.Info("Calling method '{0}.{1}()' with parameters: {2}... ", targetType, methodName, arguments);
            try
            {
                invocation.Proceed();
            }
            catch (Exception exception)
            {
                _logger.ErrorException("The method invocation failed", exception);
                throw;
            }

            _logger.Info("The method invocation succeeded: result was '{0}'.", 
                invocation.ReturnValue ?? "void");
        }

        public void LogError(string message, Exception exception)
        {
            _logger.ErrorException(message, exception);
        }
    }
}