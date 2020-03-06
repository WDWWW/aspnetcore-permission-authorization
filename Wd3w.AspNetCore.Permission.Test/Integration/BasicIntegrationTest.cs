using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Wd3w.AspNetCore.Permission.Sample;
using Xunit;

namespace Wd3w.AspNetCore.Permission.Test.Integration
{
    public class BasicFakePermissionProvider : IPermissionProvider
    {
        public IEnumerable<string> AllPermissions { get; set; }

        public IEnumerable<string> UserPermissions { get; set; }

        public Task<IEnumerable<string>> GetPermissionsFromAuthorizationContextAsync(
            AuthorizationHandlerContext context)
        {
            return Task.FromResult(UserPermissions);
        }

        public Task<IEnumerable<string>> GetAllPermissionsAsync()
        {
            return Task.FromResult(AllPermissions);
        }
    }

    public class BasicIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BasicIntegrationTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SuccessCaseIntegrationTest()
        {
            // Given
            var client = _factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services => services
                    .AddSingleton<IPermissionProvider>(new BasicFakePermissionProvider
                    {
                        AllPermissions = new []
                        {
                            "ReadableSamplePermission"
                        },
                        UserPermissions = new []
                        {
                            "ReadableSamplePermission"
                        }
                    })))
                .CreateClient();

            // When
            var response = await client.SendAsync(CreateAuthorizationPermission(client.BaseAddress));

            // Then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private static HttpRequestMessage CreateAuthorizationPermission(Uri baseAddress)
        {
            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Headers =
                {
                    {"Authorization", "Bearer abcdeabcdea"}
                },
                RequestUri = new Uri(baseAddress, "api/sample/basic-permission-api")
            };
        }

        [Fact]
        public async Task PermissionNotEnoughForAuthenticationUser()
        {
            // Given
            var client = _factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services => services
                    .AddSingleton<IPermissionProvider>(new BasicFakePermissionProvider
                    {
                        AllPermissions = new []
                        {
                            "ReadableSamplePermission"
                        },
                        UserPermissions = Enumerable.Empty<string>()
                    })))
                .CreateClient();

            // When
            var response = await client.SendAsync(CreateAuthorizationPermission(client.BaseAddress));

            // Then
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}