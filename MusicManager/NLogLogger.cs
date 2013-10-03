using System;
using System.Linq;
using Castle.DynamicProxy;
using NLog;

namespace MusicManager
{
    public class NLogLogger : IInterceptor
    {
        private readonly Logger _logger;

        public NLogLogger(Logger logger)
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

            _logger.Info("The method invocation succeeded: result was {0}.", invocation.ReturnValue);
        }
    }
}
