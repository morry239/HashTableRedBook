using System.Text;

namespace ZIPCodeSuche_RotesBuch
{
    class Program
    {
        static void Main(string[] args)
        {
            var codeSearch = new ZIPCodeSearch();
            codeSearch.Build(new StreamReader(args[0], Encoding.Default)); 
            
            Console.WriteLine($"Anwendung ZIPCodeSearch");

            string value;
            while ((value = Console.ReadLine()) != "")
            {
                var scanned = codeSearch.Classify(value);
                Console.WriteLine($"Stadt: {codeSearch.Classify(value)} unbekannt");
                Console.WriteLine($"Land:{codeSearch.Classify(value)}");
            }
        }

        
    }
}