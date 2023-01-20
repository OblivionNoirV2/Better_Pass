
using System.Threading.Tasks;
using System;

class GetWord
{
    static async Task Main()
    {

        var client = new HttpClient();
        string api_key = Environment.GetEnvironmentVariable("API_KEY_BP");
        client.DefaultRequestHeaders.Add("X-Api-Key", "api_key");
        var response = await client.GetAsync("https://api.api-ninjas.com/v1/randomword");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine($"Error: {(int)response.StatusCode} {response.ReasonPhrase}");
        };
    }
};