using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Wd3w.AspNetCore.Permission.Internal
{
    internal class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider 
    {
        private readonly IOptions<AuthorizationOptions> _options;
        private readonly IServiceProvider _serviceProvider;

        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IServiceProvider serviceProvider) : base(options)
        {
            _options = options;
            _serviceProvider = serviceProvider;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
                return policy;

            using var serviceScope = _serviceProvider.CreateScope();
            var permissionProvider = serviceScope.ServiceProvider.GetRequiredService<IPermissionProvider>();

            var permissions = await permissionProvider.GetAllPermissionsAsync();
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