using Nest;
using SearchSyncService.Constants;
using SearchSyncService.Models;

namespace SearchSyncService.ElasticSearchRepository;

public class StudentRepositoryEs
{
    public StudentRepositoryEs()
    {
        
    }

    public async Task SyncData(Student entity)
    {
        var settings = new ConnectionSettings(new Uri(EsConstants.BaseUrlEs))
            .DefaultIndex(EsConstants.IndexStudent);

        var client = new ElasticClient(settings);

        var createIndexResponse = client.Indices.Create(EsConstants.IndexStudent, c => c
            .Map<Student>(m => m.AutoMap())
        );

        var indexResponse = client.IndexDocument(entity);
    }
}