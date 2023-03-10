using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Fluff;
using System.Linq;
using System.IO;

public class GetComplexity
{
    public static int difficulty_selection;
    public static string api_key;
    static readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "api_key.txt");
    public static int Main()

    {


        if (!File.Exists(filePath))
        {
            Console.WriteLine("Enter API key: ");
            api_key = Console.ReadLine();
            File.WriteAllText(filePath, api_key);
        }
        else
        {
            api_key = File.ReadAllText(filePath);
        }

        Console.WriteLine(
            "Select a complexity level, 1-4. I for info."
            );
        //take in the readline value, check for int...
        var reply = Console.ReadLine();
        if (reply.ToUpper() == "I")
        {
            Console.WriteLine("1 = 1 word, min 8 characters, 1 uppercase, rest lowercase, 1 number, 1 special char\r\n2 = 1 word, min 10 characters, 2 uppercase, rest lower, 2 numbers, 2 special char\r\n3 = 2 words, min 12 char, 2 uppercase per word, rest lower, 3 numbers, 3 special char \r\n4 = 2 words, min 14 char, 3 uppercase per word, rest lower, 4 numbers, 4 special char\r\n");
            Main();
        }

        while (!int.TryParse(reply,
            out difficulty_selection)
            //then ensure it fits the range
            || difficulty_selection < 1
            || difficulty_selection > 4)
        {

            Console.WriteLine("Please enter a valid response");
            reply = Console.ReadLine();
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

            for (int i = 0; i < num_int_generations; i++)
            {
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


            for (int i = 0; i < num_alph_generations; i++)
            {
                //get random value from alphabet
                int alph_index = rnd_alph.Next(0, alphabet.Length);
                char alph_char = alphabet[alph_index];
                generated_alphs.Add(alph_char);

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

        string fetched_word = word_.WordFetch().Result;
        //First check that it's at least 4 characters for quality assurance purposes 
        while (fetched_word.Length < 5)
        {
            path1.MakeReadable();
        }
        //convert to readable format
        JObject jsonObject = JObject.Parse(fetched_word);
        string converted_word = (string)jsonObject["word"];

        return converted_word;

    }
    public string FuncLoop(int complexity, bool isRandom)
    {

        ChoosePath path = new ChoosePath();
        List<string> pass_components = new List<string>();
        //determine # of loops depending on complexity. >2 = 2 words
        for (int i = 0; i < (complexity > 2 ? 2 : 1); i++)
        {   //convert to a simple word and lowercase the whole word
            string word = path.MakeReadable().ToLower();
            //determine number of characters at the end to be uppercased
            int endIndex = Math.Min(word.Length, (complexity % 4) + 1);
            //make array of chars from the word 
            char[] up_you_go = word.ToCharArray();
            //uppercase according to the rules set previously
            for (int j = 1; j <= endIndex; j++)
            {
                up_you_go[word.Length - j] = char.ToUpper(up_you_go[word.Length - j]);
            }
            word = new string(up_you_go);
            pass_components.Add(word);
        }

        //set up numbers 

        List<int> generated_nums2 = new List<int>();

        switch (complexity)
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
        foreach (string str2 in char2str)
        {
            pass_components.Add(str2);
        }



        //then concatonate, randomized if that was chosen
        Filler fl = new Filler();
        if (isRandom == true)
        {
            pass_components = pass_components.OrderBy(x => Guid.NewGuid()).ToList();
            string random_test = "";
            foreach (string item in pass_components)
            {
                random_test += item;
            }
            fl.AddFiller(random_test);
            return random_test;


        }
        else
        {
            string test = string.Join("", pass_components);
            fl.AddFiller(test);
            return test;

        }
    }

};

//add stuff if the result isn't long enough
public class Filler
{
    private static List<int> add_nums;
    private static List<char> add_alph;
    private static List<char> add_sym;
    readonly Random addstuff = new Random();
    int min_length = 0;
    public void AddFiller(string item)
    {
        //Pick a random method of adding filler
        int add_choice = addstuff.Next(1, 4);
        //first determine the rules to go by
        switch (GetComplexity.difficulty_selection)
        {
            case 1:
                min_length = 8;
                break;
            case 2:
                min_length = 10;
                break;
            case 3:
                min_length = 12;
                break;
            case 4:
                min_length = 14;
                break;
        }
        //then add random strings accordingly
        while (item.Length < min_length)
        {

            switch (add_choice)
            {
                case 1: //num
                    add_nums = FluffChild.RandNums(1);
                    List<string> num2str2 = add_nums.ConvertAll(y => (string)(y + ""));
                    foreach (string str3 in num2str2)
                    {
                        //append to the existing string
                        item += str3;

                    }
                    break;
                case 2: //alph
                    add_alph = FluffChild.RandAlph(1);
                    List<string> char2str2 = add_alph.ConvertAll(z => (string)(z + ""));
                    foreach (string str4 in char2str2)
                    {
                        item += str4;
                    }
                    break;
                case 3: //sym
                    add_sym = FluffChild.RandSym(1);
                    List<string> char2str3 = add_sym.ConvertAll(x => (string)(x + ""));
                    foreach (string str5 in char2str3)
                    {
                        item += str5;

                    }
                    break;
            }

        }

        //Final output
        Console.WriteLine(item);
        Finish fn = new Finish();
        fn.End();

    }
};

public class Finish
{
    public void End()
    {
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

        var client = new HttpClient
        {
            //Timeout after 10 seconds
            Timeout = TimeSpan.FromSeconds(10)
        };
        client.DefaultRequestHeaders.Add("X-Api-Key", GetComplexity.api_key);

        var response = await client.GetAsync("https://api.api-ninjas.com/v1/randomword");
        if (response.IsSuccessStatusCode)
        {
            //wait for the async call to finish, then return response

            return await response.Content.ReadAsStringAsync();

        }
        else
        {

            Console.WriteLine($"Something went wrong with the API: {(int)response.StatusCode}");
            GetComplexity.Main();
            return "retrying";
        }
    }

};