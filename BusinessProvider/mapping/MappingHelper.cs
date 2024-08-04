using Newtonsoft.Json.Linq;

namespace BusinessProvider.Mapping;
public static class MappingHelper
{
    public static (string IndexName, JObject Mapping) LoadMappings(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        {
            var json = reader.ReadToEnd();
            var jObject = JObject.Parse(json);
            var indexName = jObject["indexName"]?.ToString();
            var mapping = jObject["mapping"] as JObject; // Use null-conditional operator to safely access the "mapping" property

            if (indexName == null || mapping == null)
            {
                throw new InvalidOperationException("The mappings file is missing the 'indexName' or 'mapping' property.");
            }

            return (indexName, mapping);
        }
    }
}