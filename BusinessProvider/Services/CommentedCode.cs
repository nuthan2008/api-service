

/*
    // Method to ensure the index exists and update mappings if necessary
    public async Task<string> EnsureIndexExistsAsync(string indexName)
    {
        var indexExistsResponse = _client.Indices.Exists(indexName);
        string result = string.Empty;
        if (!indexExistsResponse.Exists)
        {
            var createIndexResponse = await _client.Indices.CreateAsync(indexName, c => c
                .Map<T>(m => m.AutoMap().Properties(p => AddCustomMappings(p, typeof(T))))
            );

            if (!createIndexResponse.IsValid)
            {
                throw new Exception($"Failed to create index: {createIndexResponse.ServerError.Error.Reason}");
            }

            result = "Index created";
        }
        else
        {
            var putMappingResponse = await _client.MapAsync<T>(m => m
                .Index(indexName)
                .AutoMap().Properties(p => AddCustomMappings(p, typeof(T)))
            );

            if (!putMappingResponse.IsValid)
            {
                throw new Exception($"Failed to update mapping: {putMappingResponse.ServerError.Error.Reason}");
            }

            result = "Index mapping updated";
        }

        return result;
    }

    private PropertiesDescriptor<T1> AddCustomMappings<T1>(PropertiesDescriptor<T1> properties, Type type) where T1 : class
    {
        foreach (var prop in type.GetProperties())
        {
            if (prop.GetCustomAttribute<TextWithKeywordAttribute>() != null)
            {
                properties.Text(t => t
                    .Name(prop.Name)
                    .Fields(f => f
                        .Keyword(k => k.Name("keyword"))
                    )
                );
            }
            else if (prop.PropertyType.IsClass && prop.PropertyType.Namespace != null && !prop.PropertyType.Namespace.StartsWith("System"))
            {
                properties.Nested<object>(n => n
                    .Name(prop.Name)
                    .Properties(p => AddCustomMappings(p, prop.PropertyType))
                );
            }
            else if (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string))
            {
                // Handle other basic types (string, int, etc.)
                if (prop.PropertyType == typeof(string))
                {
                    properties.Text(t => t.Name(prop.Name));
                }
                else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(double) || prop.PropertyType == typeof(float) || prop.PropertyType == typeof(long) || prop.PropertyType == typeof(decimal))
                {
                    properties.Number(n => n.Name(prop.Name).Type(NumberType.Integer));
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    properties.Date(d => d.Name(prop.Name));
                }
            }
        }

        return properties;
    }
*/