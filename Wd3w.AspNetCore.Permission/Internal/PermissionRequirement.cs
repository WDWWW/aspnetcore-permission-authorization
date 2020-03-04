using Microsoft.AspNetCore.Authorization;

namespace Wd3w.AspNetCore.Permission.Internal
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; }
    }
}