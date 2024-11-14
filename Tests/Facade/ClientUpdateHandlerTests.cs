using Client;
using Client.Observer;
using System.Windows.Forms;

namespace Tests.Facade;

public class ClientUpdateHandlerTests
{
	private readonly ClientUpdateHandler _handler;

	public ClientUpdateHandlerTests()
	{
		var container = new ContainerControl();
		var mockNetworkHandler = new MockNetworkHandler();

		_handler = new ClientUpdateHandler(container, mockNetworkHandler);
	}

	[Fact]
	public void Should_Handle_KeyDown()
	{
		_handler.OnKeyDown(this, new KeyEventArgs(Keys.Down));
	}

	[Fact]
	public void Should_Handle_KeyUp()
	{
		_handler.OnKeyUp(this, new KeyEventArgs(Keys.Down));
	}

	[Fact]
	public void Should_Handle_WindowsMessage()
	{
		var message = new Message
		{
			Msg = 0x0200,
		};
		_handler.OnWindowsMessage(ref message);
	}

	[Fact]
	public void Should_Handle_Input()
	{
		var inputEvent = new InputEvent();
		_handler.HandleInput(inputEvent);
	}

	[Fact]
	public async Task Should_Update()
	{
		var updated = await _handler.UpdateAsync();
		Assert.True(updated);
	}

	[Fact]
	public async Task Should_Not_Update()
	{
		var inputEvent = new InputEvent()
		{
			KeyboardEvent = new KeyboardEvent(new KeyEventArgs(Keys.P), KeyEventType.KeyUp)
		};
		_handler.HandleInput(inputEvent);

		var updated = await _handler.UpdateAsync();
		Assert.False(updated);
	}
}
