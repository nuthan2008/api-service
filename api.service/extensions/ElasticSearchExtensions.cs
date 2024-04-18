
using BusinessProvider.Models.DataSvc;
using Nest;

namespace APIService.Extensions;

public static class ElasticSearchExtensions
{
    public static void AddElasticSearch(
        this IServiceCollection services, IConfiguration configuration
    )
    {
        var url = configuration["ElasticConfiguration:Uri"];
        var defaultIndex = configuration["ElasticConfiguration:index"];

        var settings = new ConnectionSettings(new Uri(url))
            .PrettyJson()
            .DefaultIndex(defaultIndex);

        AddDefaultMappings(settings);

        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);

        CreateIndex(client, defaultIndex);
    }

    private static void AddDefaultMappings(ConnectionSettings settings)
    {
        settings.DefaultMappingFor<Practice>(p =>
                        p.Ignore(x => x.IncorporationYear));
    }

    private static void CreateIndex(IElasticClient client, string indexName)
    {
        client.Indices.Create(indexName, i => i.Map<Practice>(x => x.AutoMap()));
    }
}
