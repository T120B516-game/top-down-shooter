namespace Backend.ChainOfResponsibility
{
    public class XmlLogger : LoggerHandler
    {
        private readonly string _filePath;

        public XmlLogger(string filePath, ILoggerHandler? nextHandler = null)
            : base(nextHandler)
        {
            _filePath = filePath;
        }

        public override void Log(string message)
        {
            var xmlMessage = $"<Log><Message>{message}</Message><Timestamp>{DateTime.Now}</Timestamp></Log>";
            File.AppendAllText(_filePath, xmlMessage + Environment.NewLine);
            base.Log(message);
        }
    }
}
