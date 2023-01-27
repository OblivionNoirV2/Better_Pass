using System;
//this enables async
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Fluff;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;

public class GetComplexity 
{
    public static int difficulty_selection;
    public static int Main() 
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
            "How many generations should it make? Limit of 10"
            );
        while (!int.TryParse(Console.ReadLine(),
            out HowManyP)
            || HowManyP > 10
            || HowManyP < 0)
        {
            Console.WriteLine("Please enter a value between 1 and 10");
        };
        
        Console.WriteLine($"Creating {HowManyP} {(HowManyP == 1 ? "password": "passwords")}...");

        ChoosePath path_ = new ChoosePath();
        //discard breaks the need for async
        path_.FuncLoop(HowManyP, GetComplexity.difficulty_selection, WantsRandom);
    }
};


//Used for symbols and any additional letters to meet the complexity requirements
namespace Fluff
{
    public class FluffChild
    {   //random numbers to append onto the word(s), each 1 digit
        public static List<int> RandNums(int num_int_generations)
        {
            List<int> generated_nums = new List<int>();
            Random rnd_num = new Random();
            //int num = rnd_num.Next(1, 10);
            for (int i = 0; i < num_int_generations; i++) {
                //limit generations to 1 digit
                int num = rnd_num.Next(1, 10);
                generated_nums.Add(num);
                Console.WriteLine(num);
            };
            //return the list from the loop
            return generated_nums;
        
        }
        public static List<char> RandAlph(int num_alph_generations)
        { 
            List<char> generated_alphs = new List<char>();
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
                char alph_char = alphabet[alph_index];
                generated_alphs.Add(alph_char);
                Console.WriteLine(alph_char);
            };
            return generated_alphs;
        }
        public static List<char> RandSym(int num_sym_generations)
        {
            List<char> generated_syms = new List<char>();

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
                char sym_char = symbols[sym_index];
                generated_syms.Add(sym_char);
                Console.WriteLine(sym_char);
            }
            return generated_syms;
        }
    };
};


public class ChoosePath
{//each generation must wait for the api call to finish before continuing

    public string MakeReadable()
    {
        GetWord word_ = new GetWord();
        //generate initial word, then do a second if the complexity requires it
        string fetched_word = word_.WordFetch().Result;
        //First check that it's at least 4 characters for quality assurance purposes 
        while (fetched_word.Length < 4)
        {
            MakeReadable();
        }
        //convert to readable format
        JObject jsonObject = JObject.Parse(fetched_word);
        string converted_word = (string)jsonObject["word"];

        return converted_word;

    }
    public void FuncLoop(int count, int complexity, bool isRandom)
    {

       // random doesn't need its own case. A couple simple ifs will suffice
        bool is_complete = false;
        ChoosePath path = new ChoosePath();
        string converted_word = path.MakeReadable();
        string final_gen = null;
        for (int i = 0; i < count; i++)
        {
        
           while(is_complete == false)
            {
                Console.WriteLine(converted_word);
                //make first or last letter uppercase
                //todo make it 50/50 chance for either
                converted_word = char.ToUpper(converted_word[0]) + converted_word.Substring(1);
                Console.WriteLine(converted_word);


                is_complete = true;

            }
     
        }
           
    }
};



public class GetWord
{

    public async Task<string> WordFetch()
    {

        var client = new HttpClient();
        //Fetch the API key from my local machine
        string api_key = Environment.GetEnvironmentVariable("API_KEY_BP");
        client.DefaultRequestHeaders.Add("X-Api-Key", api_key);
        var response = await client.GetAsync("https://api.api-ninjas.com/v1/randomword");

        if (response.IsSuccessStatusCode)
        {
            //wait for the async call to finish, then return response
            
            return await response.Content.ReadAsStringAsync();

        }
        else
        {

            Console.WriteLine($"Error: {(int)response.StatusCode} {response.ReasonPhrase}");
            return null;
        };
       

    }
   
};
