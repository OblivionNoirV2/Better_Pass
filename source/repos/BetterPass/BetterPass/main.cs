using System;
//this enables async
using System.Threading.Tasks;
using System.Net.Http;


static class GetComplexity {
    //this will only be called once per program cycle, so make it static
    static void Main() {
        Console.WriteLine(
            "Select a complexity level, 1-3. 1 will meet requirements of most sites."
            );
        //take in the readline value, check for int...
        while (!int.TryParse(Console.ReadLine(),
            //name it
            out int difficulty_selection)
            //then ensure it fits the range
            || difficulty_selection < 1
            || difficulty_selection > 3)
        {
            Console.WriteLine("Please enter a valid response");
        };
    }
};


//this should be kept accessible so multiple words can be used for more complex passwords
namespace WordSpace {
    public class GetWord {
  
        static async Task WordFetch(){

            var client = new HttpClient();
            //Fetch the API key from my local machine
            string api_key = Environment.GetEnvironmentVariable("API_KEY_BP");
            client.DefaultRequestHeaders.Add("X-Api-Key", api_key);
            var response = await client.GetAsync("https://api.api-ninjas.com/v1/randomword");

            if (response.IsSuccessStatusCode) {
                //wait for the async call to finish, then return response
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);

            } else {

                Console.WriteLine($"Error: {(int)response.StatusCode} {response.ReasonPhrase}");
            };
        }
    };
};
