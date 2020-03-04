using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Wd3w.AspNetCore.Permission
{
    public class ClaimsBasedEnumPermissionProvider<TPermissionEnum> : EnumPermissionProviderBase<TPermissionEnum>
        where TPermissionEnum : struct, Enum
    {
        public override Task<IEnumerable<string>> GetPermissionsFromAuthorizationContextAsync(AuthorizationHandlerContext context)
        {
            var permissions = context
                .User
                .Claims
                .Where(c => c.Type == EnumBasedPermissionHelper.PermissionClaimType)
                .Select(c => c.Value)
                .ToList();
            
            return Task.FromResult<IEnumerable<string>>(permissions);
        }
    }
}