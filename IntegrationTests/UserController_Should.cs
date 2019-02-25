using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UserService;
using Xunit;

namespace IntegrationTests {
    public class UserController_Should
        : IClassFixture<CustomWebApplicationFactory<Startup>> {

        public UserController_Should(CustomWebApplicationFactory<Startup> factory) {
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task Return_Cached_UserPaginated() {
            //arrange
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            //act
            var response = await Client.GetAsync("/api/Users/getUsersWithPaginationAsync?email=email45@test.com&searchString=g&pageSize=2&pageNumber=1");
            var content = await response.Content.ReadAsStringAsync();

            //assert
            //response.EnsureSuccessStatusCode();

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
            Assert.True(response.IsSuccessStatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();

            var expectedContent = "email44@test.com";
            Assert.Contains(expectedContent, content);
            //Assert.Equal(30, response.Headers.CacheControl.MaxAge.Value.TotalSeconds);
        }
    }
}
