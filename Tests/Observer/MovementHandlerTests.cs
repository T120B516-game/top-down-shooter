using Client.HubClients;
using Client.Observer;
using System.Collections.Immutable;
using System.Windows.Forms;

namespace Tests.Observer;

public class MovementHandlerTests
{
	[Fact]
	public void Should_Get_States()
	{
		var handler = new MovementHandler();
		var states = handler.GetStates();

		Assert.NotNull(states);
	}

	[Fact]
	public void Should_Handle_Empty_InputUpdate()
	{
		var handler = new MovementHandler();
		
		var inputEvent = new InputEvent();
		handler.Update(inputEvent);

		var states = handler.GetStates();
		Assert.All(states, x => Assert.False(x.Value));
	}

	[Fact]
	public void Should_Handle_Keyboard_InputUpdate()
	{
		var handler = new MovementHandler();
		
		var inputEvent = new InputEvent()
		{
			KeyboardEvent = new KeyboardEvent(new KeyEventArgs(Keys.Down), KeyEventType.KeyDown)
		};
		handler.Update(inputEvent);

		var states = handler.GetStates();
		var inactiveStates = states
			.Where(x => x.Key != Direction.Down)
			.ToList();
		var activeStates = states
			.Where(x => x.Key == Direction.Down)
			.ToList();

		Assert.All(inactiveStates, x => Assert.False(x.Value));
		Assert.All(activeStates, x => Assert.True(x.Value));
	}

	[Fact]
	public void Should_Handle_Key_Released()
	{
		var handler = new MovementHandler();

		{
			var inputEvent = new InputEvent()
			{
				KeyboardEvent = new KeyboardEvent(new KeyEventArgs(Keys.Down), KeyEventType.KeyDown)
			};
			handler.Update(inputEvent);

			var states = handler.GetStates();
			Assert.True(states[Direction.Down]);
		}
		{
			var inputEvent = new InputEvent()
			{
				KeyboardEvent = new KeyboardEvent(new KeyEventArgs(Keys.Down), KeyEventType.KeyUp)
			};
			handler.Update(inputEvent);

			var states = handler.GetStates();
			Assert.False(states[Direction.Down]);
		}
	}

	[Fact]
	public async Task Should_Send_Data()
	{
		var handler = new MovementHandler();
		var mockNetworkHandler = new MockNetworkHandler();

		var inputEvent1 = new InputEvent()
		{
			KeyboardEvent = new KeyboardEvent(new KeyEventArgs(Keys.Left), KeyEventType.KeyDown)
		};
		var inputEvent2 = new InputEvent()
		{
			KeyboardEvent = new KeyboardEvent(new KeyEventArgs(Keys.Up), KeyEventType.KeyDown)
		};

		handler.Update(inputEvent1);
		handler.Update(inputEvent2);

		await handler.SendAsync(mockNetworkHandler);
		
		var sentMovements = mockNetworkHandler.GetSentMovements();
		Assert.Contains(Direction.Left, sentMovements);
		Assert.Contains(Direction.Up, sentMovements);

		Assert.Equal(2, sentMovements.Count);
	}

	[Fact]
	public void Should_Handle_Multiple_Keyboard_InputUpdate()
	{
		var handler = new MovementHandler();

		var inputEvent1 = new InputEvent()
		{
			KeyboardEvent = new KeyboardEvent(new KeyEventArgs(Keys.Down), KeyEventType.KeyDown)
		};
		var inputEvent2 = new InputEvent()
		{
			KeyboardEvent = new KeyboardEvent(new KeyEventArgs(Keys.Right), KeyEventType.KeyDown)
		};

		handler.Update(inputEvent1);
		handler.Update(inputEvent2);

		var activeDirections = new List<Direction>
		{
			Direction.Down, Direction.Right
		};
		var inactiveDirections = new List<Direction>
		{
			Direction.Up, Direction.Left
		};

		var states = handler.GetStates();
		var inactiveStates = states
			.Where(x => inactiveDirections.Contains(x.Key))
			.ToList();
		var activeStates = states
			.Where(x => activeDirections.Contains(x.Key))
			.ToList();

		Assert.All(inactiveStates, x => Assert.False(x.Value));
		Assert.All(activeStates, x => Assert.True(x.Value));
	}

	[Theory]
	[InlineData(Keys.Up, Direction.Up)]
	[InlineData(Keys.Right, Direction.Right)]
	[InlineData(Keys.Down, Direction.Down)]
	[InlineData(Keys.Left, Direction.Left)]
	[InlineData(Keys.Z, Direction.Undo)]
	[InlineData(Keys.OemSemicolon, null)]
	public void Should_Get_Direction(Keys keyCode, Direction? direction)
	{
		var outputDirection = MovementHandler.GetDirection(keyCode);
		Assert.Equal(direction, outputDirection);
	}
}