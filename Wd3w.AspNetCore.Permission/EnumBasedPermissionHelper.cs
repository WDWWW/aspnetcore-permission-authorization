using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Wd3w.AspNetCore.Permission.Internal;

namespace Wd3w.AspNetCore.Permission
{
    public static class EnumBasedPermissionHelper
    {
        internal const string PermissionClaimType = "http://schemas.wdwww.com/2020/03/identity/claims/permissionname";
        
        public static IEnumerable<string> GetAllPermissionsFromEnum<T>() where T : struct, Enum
        {
            var type = typeof(T);
            return Enum.GetNames(type)
                .Select(name => (name, display: type.GetMember(name)[0].GetCustomAttribute<PermissionAttribute>()))
                .Where(meta => meta.display != null)
                .Select(meta => meta.display.Policy)
                .ToArray();
        }
        
        public static void AddEnumBasedPermissionServices<TEnum, TEnumPermissionProvider>(this IServiceCollection services, ServiceLifetime lifetime) 
            where TEnumPermissionProvider : EnumPermissionProviderBase<TEnum> 
            where TEnum : struct, Enum
        {
            Verify<TEnum>();
            services.AddPermissionServices<TEnumPermissionProvider>(lifetime);
        }
        
        public static void AddEnumBasedPermissionServices<TEnum, TEnumPermissionProvider>(this IServiceCollection services) 
            where TEnumPermissionProvider : EnumPermissionProviderBase<TEnum> 
            where TEnum : struct, Enum
        {
            AddEnumBasedPermissionServices<TEnum, TEnumPermissionProvider>(services, ServiceLifetime.Scoped);
        }

        public static void AddClaimEnumBasedPermissionServices<TEnum>(this IServiceCollection services, ServiceLifetime serviceLifetime) where TEnum : struct, Enum
        {
            AddEnumBasedPermissionServices<TEnum, ClaimsBasedEnumPermissionProvider<TEnum>>(services, serviceLifetime);
        }
        
        public static void AddClaimEnumBasedPermissionServices<TEnum>(this IServiceCollection services) where TEnum : struct, Enum
        {
            AddEnumBasedPermissionServices<TEnum, ClaimsBasedEnumPermissionProvider<TEnum>>(services, ServiceLifetime.Scoped);
        }

        public static void AddPermission(this ICollection<Claim> claims, string permission)
        {
            claims.Add(CreatePermissionClaim(permission));
        }

        public static Claim CreatePermissionClaim(string permission)
        {
            return new Claim(PermissionClaimType, permission);
        }

        public static string GetPermission<TEnum>(TEnum permission) where TEnum : struct, Enum
        {
            return typeof(TEnum).GetMember(permission.ToString())[0].GetCustomAttribute<PermissionAttribute>().Policy;
        }

        private static void Verify<T>() where T : struct, Enum
        {
            var duplicated = GetAllPermissionsFromEnum<T>()
                .GroupBy(p => p)
                .Where(g => g.Count() > 1)
                .ToList();

            if (duplicated.Count == 0)
                return;

            var message = duplicated
                .Select(g => g.Key)
                .JoinAsString(", ")
                .Wrap("(", ")");
            throw new ValidationException($"Some Permission members is duplicated. {message}");
        }
    }
}