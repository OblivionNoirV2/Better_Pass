using System;
//this enables async
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using WordSpace;
using Fluff;

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
//if characters are not long enough with the words, append random letters from the alphabet 
//if too long, remove the excess chars
//1 = 1 word, 8 characters, 1 uppercase, rest lowercase, 1 int, 1 special char
//2 = 1 word, 10 characters, 2 uppercase, rest lower, 2 int, 2 special char
//3 = 2 words, 12 char, 3 uppercase, rest lower, 3 int, 3 special char 
//4 = 2 words, 14 char, 4 uppercase, rest lower, 4 int, 4 special char

//if "randomly assembled" is chosen, all of these elements get randomly
//strung together instead of a simple append

public class ChoosePath
{
    public void FuncLoop(int count, int complexity, bool isRandom)
    {
        for (int i = 0; i < count; i++)
        {   //bless you, ChatGPT.
            int switch_args = (complexity) * 2 + (isRandom ? 1 : 0); 
            /*each 2 cases represents a pair. ex 1 and 2 are complexity 1
            /with false and true random, respecitvely*/
            switch (switch_args)
            {   
                case 2: //C1, random F
              
                break;
                case 3: //C1, random T

                break;
                case 4: //C2, random F

                break;
                case 5: //C2, random T

                break;
                case 6: //C3, random F

                break;
                case 7: //C3, random T

                break;
                case 8: //C4, random F

                break;
                case 9://C4, random T

                break;
                default:
                    Console.WriteLine("Shouldn't be here");
                break;
            };
        }
    }
};
/*public class ShowTime
{
    //the final results to be returned

    public static string PasswordGeneration(int complexity, bool isRandom)
    {
       List<string> FinalGens = new List<string>();
       GetWord work = new GetWord(); 

    }
};*/

namespace WordSpace 
{
    public class GetWord 
    {
  
        public async Task WordFetch()
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
