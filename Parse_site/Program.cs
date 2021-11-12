using System;
using System.Threading.Tasks;

namespace Parse_site
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.Write("Input url website: ");
                string url = Console.ReadLine();
                
                ParsingWebsite parse = new ParsingWebsite(url);

                Console.WriteLine("Starting parsing website....\n\n\n");
                await parse.Start();
            }
            catch (FormatException fex)
            {
                Console.WriteLine(fex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}