using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Driver;
using System.Net;
using System.Text.Json;
using ToDoList.Api.Models;

namespace ToDoList.Api.ApiTests;

public class GetOneTodoItemTest : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{


    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private IMongoCollection<ToDoItem> _mongoCollection;

    public GetOneTodoItemTest(WebApplicationFactory<Program> factory) {
        _factory = factory;
        _client = _factory.CreateClient();

        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var mongoDatabase = mongoClient.GetDatabase("TodoItem");
        _mongoCollection = mongoDatabase.GetCollection<ToDoItem>("todos");
    }

    public async Task InitializeAsync()
    {
        await _mongoCollection.DeleteManyAsync(FilterDefinition<ToDoItem>.Empty);
    }

    public Task DisposeAsync() => Task.CompletedTask;


    [Fact]
    public async void should_get_todo_by_given_id()
    {   
        // Arrange
        var todoItem = new ToDoItem
        {
            Id = "5f9a7d8e2d3b4a1eb8a7d8e3",
            Description = "Buy groceries",
            Done = false,
            Favorite = true
        };

        await _mongoCollection.InsertOneAsync(todoItem);
        await Task.Delay(2000);

        // Act
        var response = await _client.GetAsync("/api/v1/todoitems/5f9a7d8e2d3b4a1eb8a7d8e3");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();

        var returnedTodos = JsonSerializer.Deserialize<ToDoItemDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(returnedTodos);
        Assert.Equal("Buy groceries", returnedTodos.Description);
        Assert.True(returnedTodos.Favorite);
        Assert.False(returnedTodos.Done);
    }

    [Fact]
    public async void should_get_NOTFOUND_by_invalid_id()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync("/api/v1/todoitems/not_exist_id");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}