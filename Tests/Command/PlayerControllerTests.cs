using Shared;
using Backend.BehavioralPatterns;
using static System.Net.Mime.MediaTypeNames;

namespace Tests.Command
{
	public class PlayerControllerTests
	{
		[Fact]
		public void Move_Multiple_Up()
		{
			PlayerController controller = new PlayerController();
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 200,
				Y = 200,
				Image = "PlayerUp",
				Health = 40,
				Speed = 10,
			};
			int nextX = stubPlayer.X;
			int nextY = stubPlayer.Y;
			
			for (int i = 0; i < 10; i++)
			{
				controller.MoveUp(stubPlayer);
				nextY -= stubPlayer.Speed;
			}

			Assert.Equal(nextX, stubPlayer.X);
			Assert.Equal(nextY, stubPlayer.Y);

		}

		[Fact]
		public void Move_Multiple_Down()
		{
			PlayerController controller = new PlayerController();
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 200,
				Y = 200,
				Image = "PlayerDown",
				Health = 40,
				Speed = 10,
			};
			int nextX = stubPlayer.X;
			int nextY = stubPlayer.Y;

			for (int i = 0; i < 10; i++)
			{
				controller.MoveDown(stubPlayer);
				nextY += stubPlayer.Speed;
			}

			Assert.Equal(nextX, stubPlayer.X);
			Assert.Equal(nextY, stubPlayer.Y);
		}

		[Fact]
		public void Move_Multiple_Left()
		{
			PlayerController controller = new PlayerController();
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 200,
				Y = 200,
				Image = "PlayerLeft",
				Health = 40,
				Speed = 10,
			};
			int nextX = stubPlayer.X;
			int nextY = stubPlayer.Y;

			for (int i = 0; i < 10; i++)
			{
				controller.MoveLeft(stubPlayer);
				nextX -= stubPlayer.Speed;
			}

			Assert.Equal(nextX, stubPlayer.X);
			Assert.Equal(nextY, stubPlayer.Y);
		}

		[Fact]
		public void Move_Multiple_Right()
		{
			PlayerController controller = new PlayerController();
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 200,
				Y = 200,
				Image = "PlayerRight",
				Health = 40,
				Speed = 10,
			};
			int nextX = stubPlayer.X;
			int nextY = stubPlayer.Y;

			for (int i = 0; i < 10; i++)
			{
				controller.MoveRight(stubPlayer);
				nextX += stubPlayer.Speed;
			}

			Assert.Equal(nextX, stubPlayer.X);
			Assert.Equal(nextY, stubPlayer.Y);
		}

		[Fact]
		public void Undo_MultipleMoves()
		{
			PlayerController controller = new PlayerController();
			var stubPlayer = new Player()
			{
				Id = 0,
				X = 200,
				Y = 200,
				Image = "PlayerRight",
				Health = 40,
				Speed = 10,
			};
			int initialX = stubPlayer.X;
			int initialY = stubPlayer.Y;

			controller.MoveUp(stubPlayer);
			controller.MoveDown(stubPlayer);
			controller.MoveLeft(stubPlayer);
			controller.MoveRight(stubPlayer);
			for (int i = 0; i < 4; i++)
			{
				controller.Undo();
			}

			Assert.Equal(initialX, stubPlayer.X);
			Assert.Equal(initialY, stubPlayer.Y);
		}
	}
}
