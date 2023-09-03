namespace SearchSyncService.MongoDb;

public interface IStudentRepository
{
    Task<Student> GetByGuidId(BaseModel changeEvent);
}
