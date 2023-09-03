using Nest;

namespace SearchSyncService.ElasticSearchRepository;

public class StudentRepositoryEs : IStudentRepositoryEs
{
    private ConnectionSettings _elasticClientSettings;
    private readonly ElasticSearchOptions _options;

    public StudentRepositoryEs(IConfiguration configuration)
    {
        _options = configuration.GetSection("ElasticSearch")?.Get<ElasticSearchOptions>() ?? throw new ArgumentNullException("ElasticSearch");
        _elasticClientSettings = new ConnectionSettings(new Uri(_options.Url))
                    .DefaultIndex(_options.StudentIndex);
    }

    public async Task SyncData(Student entity)
    {
        var client = new ElasticClient(_elasticClientSettings);

        await client.Indices.CreateAsync(_options.StudentIndex, c => c
              .Map<Student>(m => m.AutoMap())
          );

        var indexResponse = client.IndexDocument(entity);
    }

    public async Task UpdateSyncData(Student entity)
    {
        var client = new ElasticClient(_elasticClientSettings);

        // Update the document with the specified ID
        await client.UpdateAsync<Student>(entity.Id, u => u
            .Doc(entity)
        );
    }

    public async Task DeleteData(string id)
    {
        var client = new ElasticClient(_elasticClientSettings);

        try
        {
            var s = await client.DeleteAsync<Student>(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        // Delete the document with the specified ID
    }
}