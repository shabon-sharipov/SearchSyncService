namespace SearchSyncService.Consumers.Models;

public class BaseModel
{
    public string GuidId { get; set; }
    public string Type { get; set; }
    public EntityChangeEventType EntityChangeEventType { get; set; }
}

public enum EntityChangeEventType
{
    Insert,
    Update,
    Delete
}