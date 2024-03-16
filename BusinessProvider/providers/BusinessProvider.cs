using System.Net.Http.Json;
using BusinessProvider.Models;
using BusinessProvider.Models.DataSvc;

namespace BusinessProvider.providers
{
    public class BusinessLogicProvider : IBusinessLogicProvider
    {
        public async Task<IApiResponse<Practice>> getDataById(string Id, CancellationToken cancellationToken)
        {

            IApiResponse<Practice> response = new ApiResponse<Practice>();
            using (var client = new HttpClient())
            {
                try
                {
                    // Define the URL you want to make a request to
                    string url = "https://cuddles.care/publicproxy/vetapp/customer-search/datasvc/_babuNBrG/en_us/practices/public/animal_hospital_highway_6";

                    // Make a GET request and get the response
                    HttpResponseMessage httpResponse = await client.GetAsync(url);


                    // Check if the response is successful (status code 200-299)
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Practice>>() ?? throw new Exception("Data not found");

                        // Print the response body
                        // Console.WriteLine(responseBody);
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle any exceptions that occurred during the request
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return response;
        }
    }

}


/*
private readonly IHttpClientFactory _httpClientFactory;
private readonly IConfiguration _configuration;

string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Specify the file path (joining the desktop path with the file name)
            string filePath = Path.Combine(desktopPath, "Young_Practice_10_years.txt");

using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (DataRequest data in request)
                {
                    var urlFormat = "https://cuddles.care/customer-search/{0}/en_us/{1}s/publicview/{2}/overview";

                    IDataResponse dataResponse = new DataResponse()
                    {
                        BusinessName = data.BusinessName,
                        IncorporationYear = data.IncorporationYear,
                        UrlFormat = string.Format(urlFormat, data.SysData.sysTenant, data.Type, data.Id)
                    };

                    writer.WriteLine("Practice Name: " + dataResponse.BusinessName);
                    writer.WriteLine("Incorporation Year: " + dataResponse.IncorporationYear);
                    writer.WriteLine("Url : " + dataResponse.UrlFormat);
                    writer.WriteLine("--------------------------------------------------");
                    writer.WriteLine("--------------------------------------------------");
                    response.Add(dataResponse);
                }
            }
*/