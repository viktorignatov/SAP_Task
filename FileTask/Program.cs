
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static bool IsValidNumber(string str)
    {
        if (string.IsNullOrEmpty(str)) return false;
        if (str[0] == '0' && str.Length > 1) return false;
        return str.All(char.IsDigit);
    }


    static List<List<int>> ReadAndValidateFile(string path)
    {
        var result = new List<List<int>>();
        var lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            var numbers = lines[i].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            var validNumbers = new List<int>();

            foreach (var number in numbers)
            {
                if (IsValidNumber(number))
                {
                    validNumbers.Add(int.Parse(number));
                }
                else
                {
                    Console.WriteLine($"Invalid number found in line {i + 1}: {number}");
                    return null;
                }
            }
            result.Add(validNumbers);
        }
        return result;
    }


    static void DisplayFile(List<List<int>> content)
    {
        foreach (var line in content)
        {
            Console.WriteLine(string.Join(" ", line));
        }
    }

    static void SwapLines(List<List<int>> content, int index1, int index2)
    {
        if (index1 < 0 || index2 < 0 || index1 >= content.Count || index2 >= content.Count)
        {
            Console.WriteLine("Invalid line indexes.");
            return;
        }

        var temp = content[index1];
        content[index1] = content[index2];
        content[index2] = temp;
    }

    static void SwapNumbers(List<List<int>> content, int x1, int y1, int x2, int y2)
    {
        if (x1 < 0 || x1 >= content.Count || x2 < 0 || x2 >= content.Count ||
            y1 < 0 || y1 >= content[x1].Count || y2 < 0 || y2 >= content[x2].Count)
        {
            Console.WriteLine("Invalid coordinates for swapping.");
            return;
        }

        var temp = content[x1][y1];
        content[x1][y1] = content[x2][y2];
        content[x2][y2] = temp;
    }

    static void ReverseLine(List<List<int>> content, int lineIndex)
    {
        if (lineIndex < 0 || lineIndex >= content.Count)
        {
            Console.WriteLine("Invalid line index.");
            return;
        }

        content[lineIndex].Reverse();
    }

    static void ReverseAllNumbers(List<List<int>> content)
    {
        var allNumbers = content.SelectMany(line => line).ToList();
        allNumbers.Reverse();

        int counter = 0;
        for (int i = 0; i < content.Count; i++)
        {
            for (int j = 0; j < content[i].Count; j++)
            {
                content[i][j] = allNumbers[counter++];
            }
        }
    }

    static void SaveToFile(List<List<int>> content, string path)
    {
        using (var writer = new StreamWriter(path))
        {
            foreach (var line in content)
            {
                writer.WriteLine(string.Join(" ", line));
            }
        }
    }

    static void Main()
    {
        Console.Write("Enter the path to the file: ");
        string filePath = Console.ReadLine();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        var fileContent = ReadAndValidateFile(filePath);
        if (fileContent == null)
        {
            Console.WriteLine("File validation failed.");
            return;
        }

        Console.WriteLine("File validated successfully.");
        DisplayFile(fileContent);

        while (true)
        {
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("1 - Swap two lines");
            Console.WriteLine("2 - Swap two numbers");
            Console.WriteLine("3 - Reverse the numbers in a line");
            Console.WriteLine("4 - Reverse all numbers in the file");
            Console.WriteLine("5 - Exit");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
            {
                Console.WriteLine("Invalid choice.");
                continue;
            }

            if (choice == 5) break;

            switch (choice)
            {
                case 1:
                    Console.Write("Enter the two line indexes to swap (0-based): ");
                    int line1 = int.Parse(Console.ReadLine());
                    int line2 = int.Parse(Console.ReadLine());
                    SwapLines(fileContent, line1, line2);
                    break;

                case 2:
                    Console.Write("Enter x1, y1, x2, y2 to swap two numbers: ");
                    int x1 = int.Parse(Console.ReadLine());
                    int y1 = int.Parse(Console.ReadLine());
                    int x2 = int.Parse(Console.ReadLine());
                    int y2 = int.Parse(Console.ReadLine());
                    SwapNumbers(fileContent, x1, y1, x2, y2);
                    break;

                case 3:
                    Console.Write("Enter the line index to reverse: ");
                    int lineIndex = int.Parse(Console.ReadLine());
                    ReverseLine(fileContent, lineIndex);
                    break;

                case 4:
                    ReverseAllNumbers(fileContent);
                    break;
            }

            DisplayFile(fileContent);
            SaveToFile(fileContent, filePath);
            Console.WriteLine("Changes saved.");
        }
    }
}
