using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Wd3w.AspNetCore.Permission.Internal
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceProvider _serviceProvider;

        public PermissionAuthorizationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var permissionProvider = serviceScope.ServiceProvider.GetRequiredService<IPermissionProvider>();

            var permissions = await permissionProvider.GetPermissionsFromAuthorizationContextAsync(context);
            if (permissions.Contains(requirement.PermissionName))
                context.Succeed(requirement);
        }
    }
}