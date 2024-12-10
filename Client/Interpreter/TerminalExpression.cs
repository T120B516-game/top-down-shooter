namespace Client.Interpreter
{
    namespace Client.Interpreter
    {
        public class TerminalExpression : AbstractExpression
        {
            private int _x;
            private int _y;

            public TerminalExpression(int x, int y)
            {
                _x = x;
                _y = y;
            }

            public override void Interpret(Context context)
            {
                // Send teleportation request to the server
                context.NetworkHandler.SendTeleportAsync(_x, _y, Globals.PersonalID).Wait();

                // Update the player's position locally
                context.PlayerAdapter.UpdatePosition(_x, _y);

                Console.WriteLine($"Player teleported to ({_x}, {_y})");
            }
        }
    }

}
