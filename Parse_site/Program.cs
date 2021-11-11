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
                //Console.WriteLine("");
                //string url = Console.ReadLine();
                string url = "https://fooobar.com";

                ParseSite parse = new ParseSite(url);

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