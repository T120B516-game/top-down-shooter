namespace Backend.ChainOfResponsibility
{
    public class TextLogger : LoggerHandler
    {
        private readonly string _filePath;

        public TextLogger(string filePath, ILoggerHandler? nextHandler = null)
            : base(nextHandler)
        {
            _filePath = filePath;
        }

        public override void Log(string message)
        {
            message = $"[Log] {DateTime.Now}: {message}";
            File.AppendAllText(_filePath, message + Environment.NewLine);
            base.Log(message);
        }
    }
}
