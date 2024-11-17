namespace TypeRacerAPI.DesignPatterns.Command
{
	public class DatabaseInvoker
	{
		private readonly List<IDatabaseCommand> _commands = new List<IDatabaseCommand>();

		public void AddCommand(IDatabaseCommand command)
		{
			_commands.Add(command);
		}

		public void ExecuteCommands()
		{
			foreach (var command in _commands)
			{
				command.Execute();
			}
			_commands.Clear();
		}
	}
}
