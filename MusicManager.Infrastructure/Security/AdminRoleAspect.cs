using System.Security.Principal;
using Castle.DynamicProxy;
using NLog;

namespace MusicManager.Infrastructure.Security
{
    public class AdminRoleAspect : IInterceptor
    {
        private const string AdminRole = "Admin";
        private readonly Logger _logger;
        private readonly IPrincipal _principal;

        public AdminRoleAspect(Logger logger, IPrincipal principal)
        {
            _logger = logger;
            _principal = principal;
        }

        public void Intercept(IInvocation invocation)
        {
            IIdentity identity = _principal.Identity;
            bool isInRole = _principal.IsInRole(AdminRole);

            if (isInRole)
            {
                _logger.Info("The user '{0}' is in '{1}' role.", identity.Name, AdminRole);
                invocation.Proceed();
            }
            else
            {
                _logger.Warn("The user '{0}' is not in '{1}' role.", identity.Name, AdminRole);
            }
        }
    }
}
