namespace Client.Interpreter
{
    public class CommandSequence : AbstractExpression
    {
        private List<AbstractExpression> _commands = new List<AbstractExpression>();

        public void AddCommand(AbstractExpression command)
        {
            _commands.Add(command);
        }

        public override void Interpret(Context context)
        {
            foreach (var command in _commands)
            {
                command.Interpret(context); // Execute each command in the sequence
            }
        }
    }
}


