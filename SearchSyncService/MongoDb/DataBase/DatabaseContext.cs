namespace SearchSyncService.MongoDb.DataBase;

public class DatabaseContext
{
    private readonly MongoDbOptions _options;
    private IMongoClient client;
    public DatabaseContext(IConfiguration configuration)
    {
        _options = configuration.GetSection("MongoDb")?.Get<MongoDbOptions>() ?? throw new ArgumentNullException("MongoDb");
        client = new MongoClient(_options.ConectionString);
    }

    public IMongoDatabase MongoDatabase() => client.GetDatabase(_options.DatabaseName);
}