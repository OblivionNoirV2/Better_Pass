using System;
//this enables async
using System.Threading.Tasks;
using System.Net.Http;


public class GetComplexity 
{
    public static int difficulty_selection;
    public static void Main() 
    {
        Console.WriteLine(
            "Select a complexity level, 1-4. 1 will meet requirements of most sites."
            );
        //take in the readline value, check for int...
        while (!int.TryParse(Console.ReadLine(),
            out difficulty_selection)
            //then ensure it fits the range
            || difficulty_selection < 1
            || difficulty_selection > 4) 
        {

            Console.WriteLine("Please enter a valid response");
        };
        //enable moving to switchyswitch
        RandomAssembly choice = new RandomAssembly();
        choice.RA();
    }
};
//determine if user wants randomized password assembly
public class RandomAssembly
{
    char yesno;
    public bool RA()
    {
        bool WantsRandom;
        Console.WriteLine(
            "Do you want the order of the password components to be randomized? Type 'y' or 'n'"
            );
        while (!char.TryParse(Console.ReadLine(), 
            out yesno)
            || yesno != 'y'
            && yesno != 'n')
        {
            Console.WriteLine("Please enter a valid response");
        };
       
        if (yesno == 'y')
        {
           WantsRandom = true;
        }
        else
        {
            WantsRandom = false;
        }
        //this might need to be async
        HowMany count = new HowMany();
        count.HM();

        return WantsRandom;

    }
};
//determine how many passwords should be generated

public class HowMany
{
    public static int HowManyP;
    public int HM()
    {
        Console.WriteLine(
            "How many generations should it make? Limit of 100"
            );
        while (!int.TryParse(Console.ReadLine(),
            out HowManyP)
            && HowManyP < 100
            && HowManyP != 0)
        {
            Console.WriteLine("Please enter a value between 1 and 100");
        };
        
        Console.WriteLine($"Creating {HowManyP} {(HowManyP == 1 ? "password": "passwords")}");
        return HowManyP;
    }
};
//determine the generation path
//takes in 1 - 4 
//if characters are not long enough with the words, append random letters from the alphabet 
//if too long, remove the excess chars
//1 = 8 characters, 1 uppercase, rest lowercase, 1 int, 1 special char
//2 = 10 characters, 2 uppercase, rest lower, 2 int, 2 special char
//3 = 2 words, 12 char, 3 uppercase, rest lower, 3 int, 3 special char 
//4 2 words, 14 char, 4 uppercase, rest lower, 4 int, 4 special char

//if "randomly assembled" is chosen, all of these elements get randomly
//strung together instead of a simple append

public class ChoosePath
{
    public void SwitchySwitch(int level, bool is_r, int gen_num)
    {
        //Console.WriteLine(GetComplexity.difficulty_selection + $" MADE IT!");
     
        switch (level)
        {
            case 1:
                if (is_r)
                {
                    //move on to the generation algorithms with the data we now have
                }
                break;

            case 2:

                break;

            case 3:

                break;

            case 4:

                break;

            default:
                Console.WriteLine("shits fucked");
                break;

        };

    }
};


namespace WordSpace 
{
    public class GetWord 
    {
  
        static async Task WordFetch()
        {

            var client = new HttpClient();
            //Fetch the API key from my local machine
            string api_key = Environment.GetEnvironmentVariable("API_KEY_BP");
            client.DefaultRequestHeaders.Add("X-Api-Key", api_key);
            var response = await client.GetAsync("https://api.api-ninjas.com/v1/randomword");

            if (response.IsSuccessStatusCode) 
            {
                //wait for the async call to finish, then return response
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);

            } else 
            {

                Console.WriteLine($"Error: {(int)response.StatusCode} {response.ReasonPhrase}");
            };
        }
    };
};
