namespace BusinessProvider.Utility;
public class FileService
{
    public async Task<IEnumerable<string>> ReadAllJsonFilesAsync(string folderPath)
    {
        // Get all .json files in the specified folder
        var filePaths = Directory.GetFiles(folderPath, "*.json", SearchOption.AllDirectories);

        var fileContents = new List<string>();

        foreach (var filePath in filePaths)
        {
            // Asynchronously read the content of each JSON file
            var json = await File.ReadAllTextAsync(filePath);
            fileContents.Add(json);
        }

        return fileContents;
    }
}