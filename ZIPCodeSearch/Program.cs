// See https://aka.ms/new-console-template for more information

using System.Text;
using IronXL;

namespace ZIPCodeSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            Program prg = new Program();
            string filePath = @"/Users/mamikawamura/Desktop/geo-data.csv";
            TextReader r = File.OpenText(filePath);
            Finder find = new Finder();
            find.AddItem("","","");
            find.Build(new StreamReader(args[0], Encoding.Default));
            find.Classify(String.Empty);//dient zur Vorhersage der über Build-Methode erzeugten Regeln
        }
    }
}