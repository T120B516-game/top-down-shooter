namespace Backend.ChainOfResponsibility
{
    public abstract class LoggerHandler : ILoggerHandler
    {
        private readonly ILoggerHandler? _nextHandler;

        protected LoggerHandler(ILoggerHandler? nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public virtual void Log(string message)
        {
            _nextHandler?.Log(message);
        }
    }
}
