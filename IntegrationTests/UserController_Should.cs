using Common.Info;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UserService;
using Xunit;
using XUnitTestProject;

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
            var response = await Client.GetAsync("/api/Users/getUsersWithPaginationAsync?email=email44@test.com&searchString=g&pageSize=2&pageNumber=1");
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

        [Fact]
        public async Task Return_UserWithNotesAsync_Should() {
            //arrange
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            //act
            var response = await Client.GetAsync("/api/Users/getUserWithNotesAsync?email=email44@test.com");
            var content = await response.Content.ReadAsStringAsync();

            //assert
            //response.EnsureSuccessStatusCode();

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
            Assert.True(response.IsSuccessStatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();

            var expectedContent = "email44@test.com";
            Assert.Contains(expectedContent, content);
        }

        [Fact]
        public async Task AddNotesForUserAsync_Should() {

            var notes = JsonConvert.SerializeObject(MockDataContext.NewNotes);

            var buffer = System.Text.Encoding.UTF8.GetBytes(notes);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //arrange
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Client.PostAsync($"/api/Users/addNotesForUser?email=email44@test.com", byteContent);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            //try {
            //    var getNewNotes =
            //         await _noteService.SaveNotesByAsync(email, newNotes);
            //    return getNewNotes;
            //} catch (Exception ex) {
            //    _logger.LogError("Exception: ", ex);
            //    return null;
            //}
        }
    }
}
