// See https://aka.ms/new-console-template for more information
class Program
{
    public static void Main()
    {
        string str = "regallager"; //ein deutscher Palindrom
        Console.WriteLine($"String: {str}");
        
        while (str.Length > 0)
        {
            Console.Write($"{str[0]}=");
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[0] == str[i])
                {
                    count++;
                }
            }
            Console.WriteLine(count);
            str = str.Replace(str[0].ToString(), string.Empty);
        }
    }
}
