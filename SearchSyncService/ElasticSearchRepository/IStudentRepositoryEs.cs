namespace SearchSyncService.ElasticSearchRepository;

public interface IStudentRepositoryEs
{
    Task SyncData(Student entity);
    Task UpdateSyncData(Student entity);
    Task DeleteData(string id);
}