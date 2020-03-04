using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Wd3w.AspNetCore.Permission
{
    public interface IPermissionProvider
    {
        public Task<IEnumerable<string>> GetPermissionsFromAuthorizationContextAsync(AuthorizationHandlerContext context);

        public Task<IEnumerable<string>> GetAllPermissionsAsync();
    }
}