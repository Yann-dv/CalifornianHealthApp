using System.Diagnostics;

namespace CalifornianHealthBookingServer;

public class InputCommands
{
    public static void Commands()
    {
        Console.WriteLine("--- Californian Health Booking Server ---");
        Console.WriteLine("Waiting for inputs: ");

        var input = Console.ReadLine()?.ToLower();

        do
        {
            switch (input)
            {
                case "help":
                    Console.WriteLine("> App Commands:");
                    Console.WriteLine(" help - display this help message");
                    Console.WriteLine(" restart - restart the program");
                    Console.WriteLine(" exit - exit the program");
                    break;
                case "clean":
                    Console.Clear();
                    break;
                case "restart":
                    Console.Clear();
                    Process.Start("dotnet", "run");
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
            Console.Write("Command: ");
            input = Console.ReadLine()?.ToLower();
        } while (input != "exit");
    }
}