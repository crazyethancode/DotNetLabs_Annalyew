using System;

class Program
{
    static void Main()
    {
        string inputText = "У меня 10 долларов и 15 яблок.";
        string modifiedText = ReduceNumbersInText(inputText);
        Console.WriteLine(inputText);
        Console.WriteLine(modifiedText);
    }

    static string ReduceNumbersInText(string text)
    {
        string result = string.Empty;
        string currentNumber = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i];

            // Если текущий символ - цифра, добавляем к текущему числу
            if (char.IsDigit(currentChar))
            {
                currentNumber += currentChar;
            }
            else
            {
                // Если текущий символ не цифра и мы собрали число, уменьшаем его
                if (currentNumber.Length > 0)
                {
                    int number = int.Parse(currentNumber);
                    int reducedNumber = number / 2;
                    result += reducedNumber; // Добавляем уменьшенное число
                    currentNumber = string.Empty; // Сбрасываем текущее число
                }

                // Добавляем текущий символ в результат
                result += currentChar;
            }
        }

        // Проверяем, осталась ли часть числа в конце текста
        if (currentNumber.Length > 0)
        {
            int number = int.Parse(currentNumber);
            int reducedNumber = number / 2;
            result += reducedNumber; // Добавляем уменьшенное число
        }

        return result;
    }
}