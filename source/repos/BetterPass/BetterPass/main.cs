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

        Console.WriteLine("Creating...");

        ChoosePath path_ = new ChoosePath();
        //discard breaks the need for async
        path_.FuncLoop(GetComplexity.difficulty_selection, WantsRandom);

    }
};

//Used for symbols and any additional letters to meet the complexity requirements
namespace Fluff
{
    public class FluffChild
    {   //random numbers to append onto the word(s), each 1 digit
        static readonly Random rnd_num = new Random();
        public static List<int> RandNums(int num_int_generations)
        {
            List<int> generated_nums = new List<int>();
            
            for (int i = 0; i < num_int_generations; i++) {
                //limit generations to 1 digit
                int num = rnd_num.Next(1, 10);
                generated_nums.Add(num);
            };
            //return the list from the loop
            return generated_nums;
        
        }
        static readonly Random rnd_alph = new Random();
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
            
           
            for (int i = 0; i < num_alph_generations; i++) { 
                //get random value from alphabet
                int alph_index = rnd_alph.Next(0, alphabet.Length);
                char alph_char = alphabet[alph_index];
                generated_alphs.Add(alph_char);
                Console.WriteLine(alph_char);
            };
            return generated_alphs;
        }
        static readonly Random rnd_sym = new Random();
        public static List<char> RandSym(int num_sym_generations)
        {
            List<char> generated_syms = new List<char>();

            char[] symbols =
            {  '~', '`', '!', '@', '#', '$', '%', '^', '&',
                '*', '(', ')', '_', '-', '+', '=', '{', '[',
                '}', ']', '|', ':', ';', '"', '<' , '>', '.',
                '?', '/'
            };
            
            for (int i = 0; i < num_sym_generations; i++)
            {
                int sym_index = rnd_sym.Next(0, symbols.Length);
                char sym_char = symbols[sym_index];
                generated_syms.Add(sym_char);
                
            }
            return generated_syms;
        }
    };
};


public class ChoosePath
{//each generation must wait for the api call to finish before continuing

    public string MakeReadable()
    {
        ChoosePath path1 = new ChoosePath();
        GetWord word_ = new GetWord();
        //generate initial word, then do a second if the complexity requires it
        string fetched_word = word_.WordFetch().Result;
        //First check that it's at least 4 characters for quality assurance purposes 
        while (fetched_word.Length < 4)
        {
            path1.MakeReadable();
        }
        //convert to readable format
        JObject jsonObject = JObject.Parse(fetched_word);
        string converted_word = (string)jsonObject["word"];

        return converted_word;

    }
    public void FuncLoop(int complexity, bool isRandom)
    {
        
        ChoosePath path = new ChoosePath();
        string converted_word = path.MakeReadable();
        List <string>final_gens = new List<string>();
        List<string> pass_components = new List<string>();

         //set up words
         converted_word = converted_word.Substring(0, converted_word.Length - 1) + 
             char.ToUpper(converted_word[converted_word.Length - 1]);

         pass_components.Add(converted_word);
         
         if (complexity > 2)
         {
             string second_word = path.MakeReadable();
             second_word = second_word.Substring(0, second_word.Length - 1) + 
                 char.ToUpper(second_word[second_word.Length - 1]); 

             pass_components.Add(second_word);
                    
         }
         //set up numbers 

         List<int> generated_nums2 = new List<int>();
                    
         switch(complexity)
         {
             case 1:
                 generated_nums2 = FluffChild.RandNums(1);
             break;
                 case 2:
                 generated_nums2 = FluffChild.RandNums(2);
             break;
                 case 3:
                 generated_nums2 = FluffChild.RandNums(3);
             break;
                 case 4:
                 generated_nums2 = FluffChild.RandNums(4);
             break;
         }
         //then convert to strings and append to pass_components
         List<string> num2str = generated_nums2.ConvertAll(x => (string)(x + ""));
         foreach (string str in num2str)
         {
             pass_components.Add(str);
         }

                //set up symbols

         List<char> generated_syms2 = new List<char>();

         switch (complexity)
         {
             case 1: 
                 generated_syms2 = FluffChild.RandSym(1);
             break;
                 case 2:
                 generated_syms2 = FluffChild.RandSym(2);
             break;
                 case 3:
                 generated_syms2 = FluffChild.RandSym(3);
             break;
                 case 4:
                 generated_syms2 = FluffChild.RandSym(4);
             break;
         }
         //then convert to strings and append to pass_components
         List<string> char2str = generated_syms2.ConvertAll(y => (string)(y + ""));
         foreach(string str2 in char2str)
         {
             pass_components.Add(str2);
         }
         //Check for minimum lengths, add filler (pick randomly from the 3) as needed

         //then concatonate, randomized if that was chosen

         //then print it out and we're done! 

         foreach(string z in pass_components)
         {
             Console.WriteLine(z);

         }
        Console.WriteLine("Again? (y if yes)");
        string restart = Console.ReadLine();
        if (restart == "y")
        {
            GetComplexity.Main();
        }
        else
        {
            Environment.Exit(0);
        }   
    }
};



public class GetWord
{

    public async Task<string> WordFetch()
    {

        var client = new HttpClient();
        //Timeout after 10 seconds
        client.Timeout = TimeSpan.FromSeconds(10);
        //Fetch the API key from my local machine
        string api_key = Environment.GetEnvironmentVariable("API_KEY_BP");
        client.DefaultRequestHeaders.Add("X-Api-Key", api_key);
        try
        {
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
            }


        }
        catch (TaskCanceledException) 
        {
            Console.WriteLine("Sorry, the request times out");
            return null;

        }

    }
   
};
