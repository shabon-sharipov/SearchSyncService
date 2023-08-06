using SearchSyncService.Models;

namespace SearchSyncService.Services.Interfaces;

public interface IStudentRepositoryEs
{
     Task SyncData(Student entity);
     Task UpdateSyncData(Student entity);
     Task DeleteData(string id);
}