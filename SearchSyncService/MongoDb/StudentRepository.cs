using System.Text.Json;
using MongoDB.Driver;
using SearchSyncService.Consumers.Models;
using SearchSyncService.Models;
using SearchSyncService.MongoDb.DataBase;

namespace SearchSyncService.MongoDb;

public class StudentRepository
{
    private EFContext _efContext;
    public StudentRepository()
    {
        _efContext = new EFContext();
    }

    public async Task<Student> GetByGuidId(BaseModel changeEvent)
    {
        var collection = _efContext.MongoDatabase().GetCollection<Student>(changeEvent.Type);
        var filter = Builders<Student>.Filter.Eq(x => x.GuidId, changeEvent.GuidId);
        var result = await collection.Find(filter).FirstOrDefaultAsync();
        return result;
    }
}