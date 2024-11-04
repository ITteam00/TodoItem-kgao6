using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoItem.Infrastructure;

public class TodoItemPo{
    [BsonId]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    //public string? Id { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
    public DateTime DueDate {  get; set; }
}