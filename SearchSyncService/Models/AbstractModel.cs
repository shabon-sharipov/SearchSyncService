using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace SearchSyncService.Models;

public  class AbstractModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("Id")]
    public string GuidId { get; set; }
    [BsonElement("FirstName")]
    public string FirstName { get; set; }
    [BsonElement("LastName")]
    public string LastName { get; set; }
    [BsonElement("Email")]
    public string Email { get; set; }
    [BsonElement("Address")]
    public string Address { get; set; }
    [BsonElement("Birthday")]
    public string Birthday { get; set; }
    [BsonElement("Course")]
    public string Course { get; set; }
    [BsonElement("Group")]
    public string Group { get; set; }
}