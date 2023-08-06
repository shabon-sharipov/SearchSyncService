using SearchSyncService.Consumers.Models;
using SearchSyncService.ElasticSearchRepository;
using SearchSyncService.MongoDb;

namespace SearchSyncService.Services;
public class StudentSync
{
    private StudentRepository _studentRepository;
    private StudentRepositoryEs _studentRepositoryEs;

    public StudentSync()
    {
        _studentRepository = new StudentRepository();
        _studentRepositoryEs = new StudentRepositoryEs();
    }

    public async Task ProcessChangeEventAsync(BaseModel changeEvent)
    {
        switch (changeEvent.EntityChangeEventType)
        {
            case EntityChangeEventType.Insert:
                await HandleInsertAsync(changeEvent);
                break;
            case EntityChangeEventType.Update:
                await HandleUpdateAsync(changeEvent);
                break;
            case EntityChangeEventType.Delete:
                await HandleDeleteAsync(changeEvent);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private async Task HandleInsertAsync(BaseModel changeEvent)
    {
        var result = await _studentRepository.GetByGuidId(changeEvent);

        await _studentRepositoryEs.SyncData(result);
    }

    private async Task HandleUpdateAsync(BaseModel changeEvent)
    {
        var result = await _studentRepository.GetByGuidId(changeEvent);

        await _studentRepositoryEs.UpdateSyncData(result);
    }

    private async Task HandleDeleteAsync(BaseModel changeEvent)
    {
        await _studentRepositoryEs.DeleteData(changeEvent.Id);
    }
}