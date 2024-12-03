using Newtonsoft.Json;

namespace Backend.ChainOfResponsibility
{
    public class JsonLogger : LoggerHandler
    {
        private readonly string _filePath;

        public JsonLogger(string filePath, ILoggerHandler? nextHandler = null)
            : base(nextHandler)
        {
            _filePath = filePath;
        }

        public override void Log(string message)
        {
            var json = JsonConvert.SerializeObject(new { Log = message, Timestamp = DateTime.Now });
            File.AppendAllText(_filePath, json + Environment.NewLine);
            base.Log(message);
        }
    }
}
