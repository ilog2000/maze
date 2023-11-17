public static class ConsoleUI
{
    public static void Init()
    {
        // Allow UTF-8 encoding
        Console.OutputEncoding = System.Text.Encoding.Unicode;
    }

    public static string AskForString(string enquiry)
    {
        Console.Write($"{enquiry}: ");
        return Console.ReadLine();
    }

    public static bool AskYN(string question)
    {
        Console.Write($"{question} (y/n): ");
        var key = Console.ReadKey();
        Console.WriteLine();
        return key.KeyChar == 'y' || key.KeyChar == 'Y';
    }
}