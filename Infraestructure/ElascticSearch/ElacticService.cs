using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Nest;

namespace Infraestructure.ElascticSearch;

public class ElacticService
{
    private readonly ElasticClient _client;

    public ElacticService(IConfiguration config)
    {
        var url = config["Elasticsearch:Url"];
        var index = config["Elasticsearch:DefaultIndex"];
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("permissions");
        _client = new ElasticClient(settings);
    }

    public async Task IndexPermissionAsync(Permission permission)
    {
        await _client.IndexDocumentAsync(permission);
    }
}