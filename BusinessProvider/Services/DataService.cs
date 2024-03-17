using System;
using BusinessProvider.Domain.Services;
using BusinessProvider.Models.DataSvc;

namespace BusinessProvider.Services
{
    public class DataService : IDataService
    {
        // Assuming some data storage or retrieval mechanism
        private readonly Dictionary<string, object> _dataStore = new Dictionary<string, object>();
        public DataService()
        {
            _dataStore.Add("1", new Practice() { Id = "1", BusinessName = "Test", IncorporationYear = 1234 });
            _dataStore.Add("2", new SoloPractices() { Id = "2", BusinessName = "Test SoloPractice", IncorporationYear = 4321 });
        }


        public async Task<object> GetDataById(string type,string id)
        {

            // Here you would typically retrieve data from some data source (e.g., database, external API)
            // For demonstration purposes, we're using a dictionary as an in-memory data store

            // Assume id is the key to retrieve data
            if (_dataStore.TryGetValue(id, out var data))
            {
                Type classType = GetClassType(type);
                // Check if the retrieved data is of the requested type
                if (classType.IsInstanceOfType(data))
                {
                    return data;
                }
                else
                {
                    // Data does not match the requested type
                    return default; // or throw an exception, depending on your requirement
                }
            }
            else
            {
                // Data not found for the specified ID
                return default;
            }
        }

        public Type GetClassType(string className)
        {
            // Assuming all your classes are in the same namespace
            var yourNameSpace = "BusinessProvider.Models.DataSvc";
            string fullClassName = $"{yourNameSpace}.{className}";

            // Use reflection to get the type by name
            Type type = Type.GetType(fullClassName)!;

            if (type == null)
            {
                throw new ArgumentException($"Class type '{className}' not found.");
            }

            return type;
        }
    }
}


