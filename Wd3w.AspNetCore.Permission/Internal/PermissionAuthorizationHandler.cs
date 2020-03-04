using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Wd3w.AspNetCore.Permission.Internal
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionProvider _provider;

        public PermissionAuthorizationHandler(IPermissionProvider provider)
        {
            _provider = provider;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var permissions = await _provider.GetPermissionsFromAuthorizationContextAsync(context);
            if (permissions.Contains(requirement.PermissionName))
                context.Succeed(requirement);
        }
    }
}