using MongoDB.Driver;
namespace SearchSyncService.MongoDb.DataBase;

public class EFContext
{
    private IMongoClient client;
    public EFContext() => client = new MongoClient("mongodb://localhost:27017/");


    public IMongoDatabase MongoDatabase() => client.GetDatabase("MongoDb");
}