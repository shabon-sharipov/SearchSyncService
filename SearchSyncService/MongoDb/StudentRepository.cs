namespace SearchSyncService.MongoDb;

public class StudentRepository : IStudentRepository
{
    private DatabaseContext _efContext;
    public StudentRepository(IConfiguration configuration)
    {
        _efContext = new DatabaseContext(configuration);
    }

    public async Task<Student> GetByGuidId(BaseModel changeEvent)
    {
        var collection = _efContext.MongoDatabase().GetCollection<Student>(changeEvent.Type);
        var filter = Builders<Student>.Filter.Eq(x => x.Id, changeEvent.Id);

        try
        {
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}