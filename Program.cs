using System;
using System.Collections.Generic;

namespace OTUS_Exceptions
{
    class Program
    {
        const int borderLength = 50;
        const int conditionNumber = 3;

        enum Severity
        {
            Warning,
            Error
        }

        static void Main(string[] args)
        {
            //Объявление объектов, необходимых для работы
            IDictionary<char, string> data = new Dictionary<char, string>();
            bool[] parseResult = new bool[conditionNumber];                
            int[] parsedNumber = new int[conditionNumber];                
            bool flag = true;

            Console.WriteLine("Решение уравнения вида: a * x^2 + b * x + c = 0");
            
            //Цикл будет повторяться до тех пор, пока пользователь не введёт корректные аргументы
            while (flag)
            {
                try
                {
                    InputData(data, parseResult, parsedNumber);
                    flag = false;
                    Calculation(parsedNumber);
                }

                //Обработка исключения, вызванного отсутствием корней
                catch (NoSolutionException)
                {
                    FormatData("", Severity.Warning, data);
                }

                //Обработка исключения, вызванного некорректными аргументами
                catch (ArgumentException)
                {
                    FormatData("", Severity.Error, data);
                    data.Clear();
                }
            }
            Console.ReadKey(true);
        }

        //Метод получения данных введёных пользователем
        private static void InputData(IDictionary<char, string> data, bool[] parseResult, int[] parsedNumber)
        {
            Console.Write("Введите значение a: ");
            data.Add('a', Console.ReadLine());
            Console.Write("Введите значение b: ");
            data.Add('b', Console.ReadLine());
            Console.Write("Введите значение c: ");
            data.Add('c', Console.ReadLine());

            //Попытка приведения данных к целочисленным значениям, в случае неудачи будет брошено исключение
            parseResult[0] = int.TryParse(data['a'], out int a);
            parseResult[1] = int.TryParse(data['b'], out int b);
            parseResult[2] = int.TryParse(data['c'], out int c);

            if (parseResult[0] == false || parseResult[1] == false || parseResult[2] == false)
                throw new ArgumentException();

            parsedNumber[0] = a;
            parsedNumber[1] = b;
            parsedNumber[2] = c;
        }

        //Метод по обработке исключений и вывода его на консоль в заданном формате
        private static void FormatData(string message, Severity severity, IDictionary<char, string> data)
        {
            string border = "";
            for (int i = 0; i < borderLength; i++)
                border += "-";

            //Обработчик исключения, вызванного некорректными аргументами с выводом аргументов на консоль
            switch (severity)
            {
                case Severity.Error:
                    message = "Введены некорректные значения следующих аргументов";
                    string auxiliaryString = "";
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(border);
                    Console.WriteLine(message.PadRight(borderLength));
                    Console.WriteLine(border);
                    if (int.TryParse(data['a'], out int a) == false)
                    {
                        auxiliaryString = "a = " + data['a'];
                        Console.WriteLine(auxiliaryString.PadRight(borderLength));
                    }
                    if (int.TryParse(data['b'], out int b) == false)
                    {
                        auxiliaryString = "b = " + data['b'];
                        Console.WriteLine(auxiliaryString.PadRight(borderLength));
                    }
                    if (int.TryParse(data['c'], out int c) == false)
                    {
                        auxiliaryString = "c = " + data['c'];
                        Console.WriteLine(auxiliaryString.PadRight(borderLength));
                    }
                    Console.WriteLine(border);
                    Console.BackgroundColor = default;
                    break;

                    //Обработчик исключения, вызванного отсутствием решений уравнения
                case Severity.Warning:
                    message = "Вещественных значений не найдено";
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(border);
                    Console.WriteLine(message.PadRight(borderLength));
                    Console.WriteLine(border);
                    Console.BackgroundColor = default;
                    break;
                default:
                    break;
            }
        }
        //Метод для вычисления корней уравнения и вывода ответов на консоль, в случае их отсутствия бросит исключение
        private static void Calculation(int[] parsedNumber)
        {
            double discridiscriminant = parsedNumber[1] * parsedNumber[1] - 4 * parsedNumber[0] * parsedNumber[2];
            try
            {
                switch (discridiscriminant)
                {
                    case < 0:
                        throw new NoSolutionException();
                    case > 0:
                        double xOne = (double)(-1 * parsedNumber[1] + Math.Sqrt(discridiscriminant)) / (2 * parsedNumber[0]);
                        double xTwo = (double)(-1 * parsedNumber[1] - Math.Sqrt(discridiscriminant)) / (2 * parsedNumber[0]);
                        Console.WriteLine($"x1 = {xOne}, x2 = {xTwo}");
                        break;
                    default:
                        double xOnly = (double)(-1 * parsedNumber[1]) / (2 * parsedNumber[0]);
                        Console.WriteLine($"x = {xOnly}");
                        break;
                }
            }
            catch (NoSolutionException)
            {

                throw;
            }

        }

    }
}
