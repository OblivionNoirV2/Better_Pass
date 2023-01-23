using System;
//this enables async
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;


public class GetComplexity 
{
    public static int difficulty_selection;
    public int Main() 
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
      
        RandomAssembly choice = new RandomAssembly();
        choice.RA();
        return difficulty_selection;
    }
};
//determine if user wants randomized password assembly
public class RandomAssembly
{
    char yesno;
    public void RA()
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
        count.HM(WantsRandom);

    }
};
//determine how many passwords should be generated

public class HowMany
{
    public int HowManyP;
    public void HM(bool WantsRandom)
    {
        Console.WriteLine(
            "How many generations should it make? Limit of 100"
            );
        while (!int.TryParse(Console.ReadLine(),
            out HowManyP)
            || HowManyP > 100
            || HowManyP < 0)
        {
            Console.WriteLine("Please enter a value between 1 and 100");
        };
        
        Console.WriteLine($"Creating {HowManyP} {(HowManyP == 1 ? "password": "passwords")}...");

        ChoosePath path_ = new ChoosePath();
        path_.FuncLoop(HowManyP, GetComplexity.difficulty_selection, WantsRandom);
    }
};

//determine the generation path
public class ChoosePath
{
   
    public void FuncLoop(int count, int complexity, bool isRandom)
    {
        for(int i = 0; i < count; i++)
        {
            //Retrieve the yesno value and generate
            //pass in complexity and yesno, count is already dealt with here
            

        }

    }
 
};
//Used for symbols and any additional letters to meet the complexity requirements
namespace Fluff
{
    public class FluffChild
    {   //random numbers to append onto the word(s), each 1 digit
        public static void RandNum(int num_int_generations)
        {
            Random rnd_num = new Random();
            int num = rnd_num.Next(1, 10);
            for (int i = 0; i < num_int_generations; i++) {
                //limit generations to 1 digit
                num = rnd_num.Next(1, 10);//retrieve this later
            };
        }
        public static void RandAlph(int num_alph_generations)
        { 
            char[] alphabet = 
            {
                'a', 'b', 'c', 'd', 'e',
                'f', 'g', 'h', 'i', 'j',
                'k', 'l', 'm', 'n', 'o',
                'p', 'q', 'r', 's', 't',
                'u', 'v', 'w', 'x', 'y', 'z' 
            };
            Random rnd_alph = new Random();
           
            for (int i = 0; i < num_alph_generations; i++) { 
                //get random value from alphabet
                int alph_index = rnd_alph.Next(0, alphabet.Length);
                char alph_char = alphabet[alph_index];//retrieve this later
            }
        }
        public static void RandSym(int num_sym_generations)
        {

            char[] symbols =
            {  '~', '`', '!', '@', '#', '$', '%', '^', '&',
                '*', '(', ')', '_', '-', '+', '=', '{', '[',
                '}', ']', '|', ':', ';', '"', '<' , '>', '.',
                '?', '/'
            };
            Random rnd_sym = new Random();
            for (int i = 0; i < num_sym_generations; i++)
            {
                int sym_index = rnd_sym.Next(0, symbols.Length);
                char sym_char = symbols[sym_index];//retrieve later
            }
        }
    };
};
//if characters are not long enough with the words, append random letters from the alphabet 
//if too long, remove the excess chars
//1 = 1 word, 8 characters, 1 uppercase, rest lowercase, 1 int, 1 special char
//2 = 1 word, 10 characters, 2 uppercase, rest lower, 2 int, 2 special char
//3 = 2 words, 12 char, 3 uppercase, rest lower, 3 int, 3 special char 
//4 2 words, 14 char, 4 uppercase, rest lower, 4 int, 4 special char

//if "randomly assembled" is chosen, all of these elements get randomly
//strung together instead of a simple append
public class ShowTime
{
    //the final results to be returned
    List<string> FinalGens = new List<string>();
    public static string PasswordGeneration(int complexity, bool isRandom)
    {

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
