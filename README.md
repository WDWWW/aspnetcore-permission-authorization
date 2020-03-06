# Wd3w.AspNetCore.Permission 

> Simplify ASP.NET Core authorization based on permission

![Build&Test](https://github.com/WDWWW/aspnetcore-permission-authorization/workflows/Build&Test/badge.svg)
[![Nuget](https://img.shields.io/nuget/v/Wd3w.AspNetCore.Permission)](https://www.nuget.org/packages/Wd3w.AspNetCore.Permission/)

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
    services.AddAuthorization();
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
    services.AddAuthorization();
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

As conventionally, we can use claims of `ClaimsPricipal` to check permission of user.


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

#### 3. Register authorization components on Startup.cs

```c#
// on Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // Add your custom 
    services.AddAuthorization();
    services.AddClaimEnumBasedPermissionServices<Permissions>(); // register service scoped as default 
    // services.AddPermissionServices<CustomPermissionProvider>(ServiceLifetime.Singleton);
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseAuthorization();
}
```

#### 4. Attach `CheckPermissionAttribute` to your action method.
```c#
[CheckPermission(Permissions.ReadableSomeResource)]
[HttpGet]
public Task<ActionResult> GetSomeResourceAsync() 
{
}
```

#### 5. Put `EnumBasedPermissionHelper.PermissionClaimType` type claims to your own auth service.


```c#

public async Task<ClaimsPrincipal> GetClaimsPrincialAsync(string token)
{
    // somewhere your auth service that create principal of authenticated user.
    return new ClaimsPrincipal(new ClaimsIdentity(new [] {
        new Claims(EnumBasedPermissionHelper.PermissionClaimType, "ReadableSomeResource") // here!
    }));
}
```

## Versioning
This package versioning strategy is following AspNetCore package version. When there are new release about AspNetCore, This package also will be update.

Only minor version is mismatched AspNetCore, that will use for fixing bugs and some changes.

## License
MIT License

Copyright (c) 2020 WDWWW

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
