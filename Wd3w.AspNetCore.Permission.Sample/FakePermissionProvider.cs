using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Wd3w.AspNetCore.Permission.Sample
{
    public class FakePermissionProvider : IPermissionProvider
    {
        public Task<IEnumerable<string>> GetPermissionsFromAuthorizationContextAsync(AuthorizationHandlerContext context)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllPermissionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}