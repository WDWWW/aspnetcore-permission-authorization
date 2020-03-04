using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Wd3w.AspNetCore.Permission
{
    public abstract class EnumPermissionProviderBase<TPermissionEnum> : IPermissionProvider where TPermissionEnum : struct, Enum
    {
        protected readonly IEnumerable<string> Permissions = EnumBasedPermissionHelper
            .GetAllPermissionsFromEnum<TPermissionEnum>()
            .ToList();

        public abstract Task<IEnumerable<string>> GetPermissionsFromAuthorizationContextAsync(AuthorizationHandlerContext context);

        public Task<IEnumerable<string>> GetAllPermissionsAsync()
        {
            return Task.FromResult(Permissions);
        }
    }

    public enum Permissions
    {
        
    }
    
public class CustomPermissionProvider : EnumPermissionProviderBase<Permissions>
{
    public override Task<IEnumerable<string>> GetPermissionsFromAuthorizationContextAsync(AuthorizationHandlerContext context)
    {
        throw new NotImplementedException();
    }
}
}