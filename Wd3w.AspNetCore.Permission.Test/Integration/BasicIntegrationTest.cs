using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Wd3w.AspNetCore.Permission.Sample;
using Wd3w.TokenAuthentication;
using Xunit;

namespace Wd3w.AspNetCore.Permission.Test.Integration
{
    public class BasicFakePermissionProvider : IPermissionProvider
    {
        public Task<IEnumerable<string>> GetPermissionsFromAuthorizationContextAsync(
            AuthorizationHandlerContext context)
        {
            return Task.FromResult((IEnumerable<string>) new[]
            {
                "ReadableSamplePermission"
            });
        }

        public Task<IEnumerable<string>> GetAllPermissionsAsync()
        {
            return Task.FromResult((IEnumerable<string>) new[]
            {
                "ReadableSamplePermission"
            });
        }
    }

    public class IntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public IntegrationTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test1()
        {
            // Given
            var client = _factory
                .WithWebHostBuilder(builder => builder
                    .ConfigureTestServices(services => services
                        .AddScoped<IPermissionProvider, BasicFakePermissionProvider>()))
                .CreateClient();

            // When
            var response = await client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Headers =
                {
                    {"Authorization", "Bearer abcdeabcdea"}
                },
                RequestUri = new Uri(client.BaseAddress, "api/sample/basic-permission-api")
            });

            // Then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}