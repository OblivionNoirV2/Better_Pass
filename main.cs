
class GetWord
{
    static async Task Main()
    {

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "");
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