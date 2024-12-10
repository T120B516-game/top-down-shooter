using Client.Interpreter;
using Client.Interpreter.Client.Interpreter;

namespace Client
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Thread consoleThread = new Thread(() =>
            //{
            //    // Initialize dependencies
            //    HubConnection hubConnection = new HubConnectionBuilder()
            //        .WithUrl("http://127.0.0.1:5270/hubs:main")
            //        .Build();
            //    hubConnection.StartAsync().Wait();

            //    INetworkHandler networkHandler = new NetworkHandler(hubConnection);
            //    PlayerAdapter player = Globals.ThisPlayer;

            //    // Create the context with the network handler and player
            //    Context context = new Context(player, networkHandler);

            //    OpenConsole(context);
            //});

            //consoleThread.Start();

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());


        }

        static void OpenConsole(Context context)
        {
            AllocConsole();

            Console.WriteLine("Welcome to the teleportation command system!");
            Console.WriteLine("Type 'teleport <x> <y>' to teleport the player (e.g., 'teleport 100 200').");
            Console.WriteLine("Type 'exit' to quit.");

            while (true)
            {
                Console.Write("\nEnter a command: ");
                string commandInput = Console.ReadLine()?.Trim();

                if (commandInput == null || commandInput.ToLower() == "exit")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }

                // Parse the command and execute it using the interpreter pattern
                var parts = commandInput.Split(' ');

                if (parts[0].ToLower() == "teleport" && parts.Length == 3)
                {
                    if (int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
                    {
                        // Create a TeleportCommand and interpret it
                        TeleportCommand teleportCommand = new TeleportCommand(x, y);
                        teleportCommand.Interpret(context);
                    }
                    else
                    {
                        Console.WriteLine("Invalid coordinates. Please enter valid integers for x and y.");
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                }
            }

            Console.ReadKey();
        }

        // Allocates a new console window for the application
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
    }
}
