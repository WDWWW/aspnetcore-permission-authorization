using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Wd3w.AspNetCore.Permission.Internal
{
    internal class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider 
    {
        private readonly IOptions<AuthorizationOptions> _options;
        private readonly IPermissionProvider _provider;

        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IPermissionProvider provider) : base(options)
        {
            _options = options;
            _provider = provider;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
                return policy;

            var permissions = await _provider.GetAllPermissionsAsync();
            if (!permissions.Contains(policyName))
            	throw new ArgumentException(policyName + " is not pre-defined permission name.");

            var newPermissionPolicy = new AuthorizationPolicyBuilder()
            	.AddRequirements(new PermissionRequirement(policyName))
            	.Build();

            _options.Value.AddPolicy(policyName, newPermissionPolicy);
            return newPermissionPolicy;
        }
    }
}