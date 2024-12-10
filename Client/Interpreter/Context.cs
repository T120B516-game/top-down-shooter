using Client.HubClients;

namespace Client.Interpreter
{
    public class Context
    {
        public PlayerAdapter PlayerAdapter { get; set; }
        public INetworkHandler NetworkHandler { get; set; }

        public Context(PlayerAdapter playerAdapter, INetworkHandler networkHandler)
        {
            PlayerAdapter = playerAdapter;
            NetworkHandler = networkHandler;
        }
    }
}
