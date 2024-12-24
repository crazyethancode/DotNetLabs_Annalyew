using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        string inputText = "У меня 10 долларов и 15 яблок.";
        string resultText = ReduceNumbersInText(inputText);
        Console.WriteLine(inputText);
        Console.WriteLine(resultText);
    }

    static string ReduceNumbersInText(string text)
    {
        string pattern = @"\d+";
        return Regex.Replace(text, pattern, m =>
        {
            int number = int.Parse(m.Value) / 2;
            return number.ToString();
        });
    }
}