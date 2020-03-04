using System;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace Wd3w.AspNetCore.Permission
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class PermissionAttribute : AuthorizeAttribute
    {
        public PermissionAttribute(string permissionName) : base(permissionName)
        {
        }
    }
}