using movie_api.ApplicationLayer.UtilityService.Interface;
using System.Net.Http.Headers;

namespace movie_api.ApplicationLayer.UtilityService.Implementation
{
    public class GenericService : IGenericService
    {
        public async Task<string> ConsumeGetApi(string apiEndPoint, Dictionary<string, string> headers)
        {

            var apiResponse = "";

            // var data = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, apiEndPoint))
                {
                    if (headers != null && headers.Count > 0)
                    {
                        foreach (var header in headers)
                        {
                            request.Headers.Authorization = new AuthenticationHeaderValue($"{header.Key}", header.Value);
                        }
                    }


                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    // request.Content = data;
                    var response = await httpClient.SendAsync(request);
                    apiResponse = await response.Content.ReadAsStringAsync();
                }
                return apiResponse;
            }
        }
    }
}
