using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Wd3w.AspNetCore.Permission.Internal;

namespace Wd3w.AspNetCore.Permission
{
    public static class PermissionHelper
    {
        /// <summary>
        ///    Register system components for enabling authorization based on permission
        /// </summary>
        /// <param name="services"></param>
        /// <param name="lifetime"></param>
        /// <typeparam name="TPermissionProvider"></typeparam>
        public static void AddPermissionServices<TPermissionProvider>(this IServiceCollection services, ServiceLifetime lifetime) where TPermissionProvider : class, IPermissionProvider
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.Add(new ServiceDescriptor(typeof(IPermissionProvider), typeof(TPermissionProvider), lifetime));
        }
        
        public static void AddPermissionServices<TPermissionProvider>(this IServiceCollection services) where TPermissionProvider : class, IPermissionProvider
        {
            AddPermissionServices<TPermissionProvider>(services, ServiceLifetime.Scoped);
        }
    }
}