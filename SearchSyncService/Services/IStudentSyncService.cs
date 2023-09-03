namespace SearchSyncService.Services;

public interface IStudentSyncService
{
    Task ProcessChangeEventAsync(BaseModel changeEvent);
}
