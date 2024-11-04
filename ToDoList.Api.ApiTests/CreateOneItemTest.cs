using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Driver;
using System.Net;
using System.Text;
using System.Text.Json;
using ToDoList.Api.Models;

namespace ToDoList.Api.ApiTests
{
    public class CreateOneItemTest : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private IMongoCollection<ToDoItem> _mongoCollection;

        public CreateOneItemTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("ToDoItems");
            _mongoCollection = mongoDatabase.GetCollection<ToDoItem>("ToDoItems");
        }

        public async Task InitializeAsync()
        {
            await _mongoCollection.DeleteManyAsync(FilterDefinition<ToDoItem>.Empty);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async void should_create_todoitem()
        {
            // Arrange
            var todoItem = new ToDoItem
            {
                Id = "5f9a7d8e2d3b4a1eb8a7d8e2",
                Description = "test create",
                Done = false,
                Favorite = true
            };

            var json = JsonSerializer.Serialize(todoItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/todoitems", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var returnedTodos = JsonSerializer.Deserialize<ToDoItemDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(returnedTodos);
            Assert.Equal("test create", returnedTodos.Description);
            Assert.True(returnedTodos.Favorite);
            Assert.False(returnedTodos.Done);
        }

    }
}
