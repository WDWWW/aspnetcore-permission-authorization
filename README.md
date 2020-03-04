# Wd3w.AspNetCore.Permission 

> Simplify ASP.NET Core authorization based on permission

## Features

- Permission authorization.
- Enum based permission authorization.
- Claim, enum based permission authorization.

## Getting Start

### Installation

```
dotnet add package Wd3w.AspNetCore.Permission
```

### Basic permission authorization

If your permissions are constantly changing, this strategy is for you.

#### 1. Implement your custom permission provider about `IPermissionNameProvider`

```c#
public class CustomPermissionProvider : IPermissionNameProvider
{
    public async Task<IEnumerable<string>> GetPermissionsFromAuthorizationContextAsync(AuthorizationHandlerContext context)
    {
        // do something...
    }

    public async Task<IEnumerable<string>> GetAllPermissionsAsync()
    {
        // return all permissions
    }
}
```


#### 2. Register authorization components on Startup.cs

```c#
// on Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // Add your custom 
    services.AddPermissionServices<CustomPermissionProvider>(); // register service scoped as default 
    // services.AddPermissionServices<CustomPermissionProvider>(ServiceLifetime.Singleton);
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseAuthorization();
}
```

#### 3. Attach `PermissionAttribute` to your action method.

```c#
// on some controller

[Permission("ReadSomeResource")]
[HttpGet]
public Task<ActionResult> GetSomeResourceAsync() 
{
}
```

### Enum permission based authorization

If you need(or want) to setup **type-safe** permission authorization, this strategy is good option.
Type-Safe permission is meaning all permission managed by source code.

#### 1. Write your own permission `Enum`

```c#
public enum Permissions
{
    [Permission("ReadableSomeResource")]
    ReadableSomeResource
}
```

#### 2. Write your enum based permission filter attribute by extending `PermissionAttribute`

```c#
public class CheckPermissionAttribute : PermissionAttribute
{
    public CheckPermissionAttribute(Permissions permission) : base(EnumBasedPermissionHelper.GetPermission(permission))
    {
    }
}
```

#### 3. Implement your own permission provider by extending `EnumPermissionProviderBase` 

```c#
public class CustomPermissionProvider : EnumPermissionProviderBase<Permissions>
{
    public override Task<IEnumerable<string>> GetPermissionsFromAuthorizationContextAsync(AuthorizationHandlerContext context)
    {
        // do something..
    }
}
```

#### 4. Register authorization components on Startup.cs

```c#
// on Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // Add your custom 
    services.AddEnumBasedPermissionServices<Permissions, CustomPermissionProvider>(); // register service scoped as default 
    // services.AddPermissionServices<CustomPermissionProvider>(ServiceLifetime.Singleton);
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseAuthorization();
}
```

#### 5. 3. Attach `CheckPermissionAttribute` to your action method.
```c#
[CheckPermission(Permissions.ReadableSomeResource)]
[HttpGet]
public Task<ActionResult> GetSomeResourceAsync() 
{
}
```

### Claim, Enum permission based authorization


 on writing...
 