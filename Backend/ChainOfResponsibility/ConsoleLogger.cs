namespace Backend.ChainOfResponsibility
{
    public class ConsoleLogger : LoggerHandler
    {
        public ConsoleLogger(ILoggerHandler? nextHandler = null)
            : base(nextHandler)
        {
        }

        public override void Log(string message)
        {
            Console.WriteLine($"[Log] {DateTime.Now}: {message}");
            base.Log(message);
        }
    }
}
