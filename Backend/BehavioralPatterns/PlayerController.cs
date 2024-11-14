using Shared;
using System.Numerics;

namespace Backend.BehavioralPatterns
{
	public class PlayerController
	{
		private readonly SemaphoreSlim _semaphorSlim = new(1, 1);
		private LinkedList<ICommand> CommandList = new LinkedList<ICommand>();
		private int maxListSize = 50;

		public void MoveUp(Player target)
		{
			ICommand cmd = new UpCommand(target);
			cmd.Move();
			_semaphorSlim.WaitAsync();
			CommandList.AddLast(cmd);
			if (CommandList.Count == maxListSize)
				CommandList.RemoveFirst();
			_semaphorSlim.Release();
		}

		public void MoveDown(Player target)
		{
			ICommand cmd = new DownCommand(target);
			cmd.Move();
			_semaphorSlim.WaitAsync();
			CommandList.AddLast(cmd);
			if (CommandList.Count == maxListSize)
				CommandList.RemoveFirst();
			_semaphorSlim.Release();
		}

		public void MoveLeft(Player target)
		{
			ICommand cmd = new LeftCommand(target);
			cmd.Move();
			_semaphorSlim.WaitAsync();
			CommandList.AddLast(cmd);
			if (CommandList.Count == maxListSize)
				CommandList.RemoveFirst();
			_semaphorSlim.Release();
		}

		public void MoveRight(Player target)
		{
			ICommand cmd = new RightCommand(target);
			cmd.Move();
			_semaphorSlim.WaitAsync();
			CommandList.AddLast(cmd);
			if(CommandList.Count == maxListSize)
				CommandList.RemoveFirst();
			_semaphorSlim.Release();
		}

		public void Undo()
		{
			if(CommandList.Count > 0)
			{
				_semaphorSlim.WaitAsync();
				ICommand cmd = CommandList.Last();
				cmd.Undo();
				CommandList.RemoveLast();
				_semaphorSlim.Release();
			}
			
		}
	}

	public interface ICommand
	{
		void Move();
		void Undo();
	}

	public class UpCommand : ICommand
	{
		Player ControlTarget;
		int? previousPosition;
		string previousImage;

		public UpCommand(Player controlTarget) 
		{
			previousImage = null;
			previousPosition = null;
			this.ControlTarget = controlTarget;
		}
		public void Move()
		{
			previousImage = ControlTarget.Image;
			previousPosition = ControlTarget.Y;
			ControlTarget.Y -= ControlTarget.Speed;
			ControlTarget.Image = "PlayerUp";
		}

		public void Undo()
		{
			if (previousPosition != null)
			{
				ControlTarget.Y = (int)previousPosition;
				ControlTarget.Image = previousImage;
				previousPosition = null;
				previousImage = null;
			}
		}
	}
	public class DownCommand : ICommand
	{
		Player ControlTarget;
		int? previousPosition;
		string previousImage;

		public DownCommand(Player controlTarget)
		{
			this.ControlTarget = controlTarget;
		}
		public void Move()
		{
			previousImage = ControlTarget.Image;
			previousPosition = ControlTarget.Y;
			ControlTarget.Y += ControlTarget.Speed;
			ControlTarget.Image = "PlayerDown";
		}

		public void Undo()
		{
			if (previousPosition != null)
			{
				ControlTarget.Y = (int)previousPosition;
				ControlTarget.Image = previousImage;
				previousPosition = null;
				previousImage = null;
			}
		}
	}
	public class LeftCommand : ICommand
	{
		Player ControlTarget;
		int? previousPosition;
		string previousImage;

		public LeftCommand(Player controlTarget)
		{
			this.ControlTarget = controlTarget;
		}
		public void Move()
		{
			previousImage = ControlTarget.Image;
			previousPosition = ControlTarget.X;
			ControlTarget.X -= ControlTarget.Speed;
			ControlTarget.Image = "PlayerLeft";
		}

		public void Undo()
		{
			if (previousPosition != null)
			{
				ControlTarget.X = (int)previousPosition;
				ControlTarget.Image = previousImage;
				previousPosition = null;
				previousImage = null;
			}
		}
	}
	public class RightCommand : ICommand
	{
		Player ControlTarget;
		int? previousPosition;
		string previousImage;

		public RightCommand(Player controlTarget)
		{
			this.ControlTarget = controlTarget;
		}
		public void Move()
		{
			previousImage = ControlTarget.Image;
			previousPosition = ControlTarget.X;
			ControlTarget.X += ControlTarget.Speed;
			ControlTarget.Image = "PlayerRight";
		}

		public void Undo()
		{
			if (previousPosition != null)
			{
				ControlTarget.X = (int)previousPosition;
				ControlTarget.Image = previousImage;
				previousPosition = null;
				previousImage = null;
			}
		}
	}
}
