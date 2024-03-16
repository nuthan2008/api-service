namespace BusinessProvider;
using System;
using Nest;

public interface IElasticSearchIndexProvider
{

}
public class ElasticSearchIndexProvider : IElasticSearchIndexProvider
{
    public ElasticSearchIndexProvider()
    {

    }

    public async Task CreateIndex(string indexName)
    {
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex(indexName);

        var client = new ElasticClient(settings);

        var indexSettings = new IndexState
        {
            Settings = new IndexSettings
            {
                NumberOfShards = 1,
                NumberOfReplicas = 0
            },
            Mappings = new TypeMapping
            {
                Properties = new Properties
                {
                    { "id", new KeywordProperty() },
                    {
                        "name", new TextProperty
                        {
                            Analyzer = "standard"
                        }
                    },
                    {
                        "keyword_field", new KeywordProperty()
                    },
                    { "pan", new NumberProperty()},
                    {
                        "dob", new DateProperty
                        {
                            Format = "yyyy-MM-dd"
                        }
                    },
                    {
                        "isPublished", new BooleanProperty()
                    }
                    // Define other properties as needed
                }
            }
        };

        var createIndexResponse = client.Indices.Create(indexName, c => c
            .InitializeUsing(indexSettings)
            .Map<Document>(m => m.AutoMap())
        );

        if (createIndexResponse.IsValid)
        {
            Console.WriteLine("Index created successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to create index: {createIndexResponse.ServerError.Error}");
        }
    }
}

public class Document
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}