using Backend.BehavioralPatterns;
using Shared;

namespace Tests.Command
{
	public class CommandTests
	{
		[Fact]
		public void UpCommand_Move_DataChanged()
		{
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 100,
				Y = 100,
				Image = "PlayerLeft",
				Health = 40,
				Speed = 10,
			};
			int nextX = stubPlayer.X;
			int nextY = stubPlayer.Y - stubPlayer.Speed;
			string nextImage = "PlayerUp";
			ICommand command = new UpCommand(stubPlayer);

			command.Move();

			Assert.Equal(nextX, stubPlayer.X);
			Assert.Equal(nextY, stubPlayer.Y);
			Assert.Equal(nextImage, stubPlayer.Image);
		}

		[Fact]
		public void DownCommand_Move_DataChanged()
		{
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 100,
				Y = 100,
				Image = "PlayerLeft",
				Health = 40,
				Speed = 10,
			};
			int nextX = stubPlayer.X;
			int nextY = stubPlayer.Y + stubPlayer.Speed;
			string nextImage = "PlayerDown";
			ICommand command = new DownCommand(stubPlayer);

			command.Move();

			Assert.Equal(nextX, stubPlayer.X);
			Assert.Equal(nextY, stubPlayer.Y);
			Assert.Equal(nextImage, stubPlayer.Image);
		}

		[Fact]
		public void LeftCommand_Move_DataChanged()
		{
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 100,
				Y = 100,
				Image = "PlayerUp",
				Health = 40,
				Speed = 10,
			};
			int nextX = stubPlayer.X - stubPlayer.Speed;
			int nextY = stubPlayer.Y;
			string nextImage = "PlayerLeft";
			ICommand command = new LeftCommand(stubPlayer);

			command.Move();

			Assert.Equal(nextX, stubPlayer.X);
			Assert.Equal(nextY, stubPlayer.Y);
			Assert.Equal(nextImage, stubPlayer.Image);
		}

		[Fact]
		public void RightCommand_Move_DataChanged()
		{
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 100,
				Y = 100,
				Image = "PlayerLeft",
				Health = 40,
				Speed = 10,
			};
			int nextX = stubPlayer.X + stubPlayer.Speed;
			int nextY = stubPlayer.Y;
			string nextImage = "PlayerRight";
			ICommand command = new RightCommand(stubPlayer);

			command.Move();

			Assert.Equal(nextX, stubPlayer.X);
			Assert.Equal(nextY, stubPlayer.Y);
			Assert.Equal(nextImage, stubPlayer.Image);
		}


		[InlineData("up")]
		[InlineData("down")]
		[InlineData("left")]
		[InlineData("right")]
		[Theory]
		public void Undo_PositionSame(string type)
		{
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 100,
				Y = 100,
				Image = "PlayerUp",
				Health = 40,
				Speed = 10,
			};
			int prevX = stubPlayer.X;
			int prevY = stubPlayer.Y;
			ICommand command;
			switch (type)
			{
				case "up":
					command = new UpCommand(stubPlayer);
					break;
				case "down":
					command = new DownCommand(stubPlayer);
					break;
				case "left":
					command = new LeftCommand(stubPlayer);
					break;
				case "right":
					command = new RightCommand(stubPlayer);
					break;
				default:
					throw new ArgumentException();
			}

			command.Move();
			command.Undo();

			Assert.Equal(prevX, stubPlayer.X);
			Assert.Equal(prevY, stubPlayer.Y);
		}

		[InlineData("up")]
		[InlineData("down")]
		[InlineData("left")]
		[InlineData("right")]
		[Theory]
		public void Undo_ImageSame(string type)
		{
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 100,
				Y = 100,
				Image = "PlayerUp",
				Health = 40,
				Speed = 10,
			};

			string prevImage = stubPlayer.Image;

			ICommand command;
			switch (type)
			{
				case "up":
					command = new UpCommand(stubPlayer);
					break;
				case "down":
					command = new DownCommand(stubPlayer);
					break;
				case "left":
					command = new LeftCommand(stubPlayer);
					break;
				case "right":
					command = new RightCommand(stubPlayer);
					break;
				default:
					throw new ArgumentException();
			}

			command.Move();
			command.Undo();

			Assert.Equal(prevImage, stubPlayer.Image);
		}
	}
}
