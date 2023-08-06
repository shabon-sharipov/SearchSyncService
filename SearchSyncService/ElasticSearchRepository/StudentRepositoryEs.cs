using Nest;
using SearchSyncService.Constants;
using SearchSyncService.Models;
using SearchSyncService.Services.Interfaces;

namespace SearchSyncService.ElasticSearchRepository;

public class StudentRepositoryEs : IStudentRepositoryEs
{
    private IStudentRepositoryEs _studentRepositoryEsImplementation;

    public StudentRepositoryEs()
    {
    }

    public async Task SyncData(Student entity)
    {
        var settings = new ConnectionSettings(new Uri(EsConstants.BaseUrlEs))
            .DefaultIndex(EsConstants.IndexStudent);

        var client = new ElasticClient(settings);

        client.Indices.Create(EsConstants.IndexStudent, c => c
            .Map<Student>(m => m.AutoMap())
        );

        var indexResponse = client.IndexDocument(entity);
    }

    public async Task UpdateSyncData(Student entity)
    {
        var settings = new ConnectionSettings(new Uri(EsConstants.BaseUrlEs))
            .DefaultIndex(EsConstants.IndexStudent);

        var client = new ElasticClient(settings);

        // Update the document with the specified ID
        await client.UpdateAsync<Student>(entity.Id, u => u
            .Doc(entity)
        );
    }

    public async Task DeleteData(string id)
    {
        var settings = new ConnectionSettings(new Uri(EsConstants.BaseUrlEs))
            .DefaultIndex(EsConstants.IndexStudent);

        var client = new ElasticClient(settings);

        try
        {
        var s=     await client.DeleteAsync<Student>(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        // Delete the document with the specified ID
    }
}