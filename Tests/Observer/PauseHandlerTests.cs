using Client.Observer;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace Tests.Observer;

public class PauseHandlerTests
{
	[Fact]
	public void Should_Initialize()
	{
		var handler = new PauseHandler();
		
		var randomControl = new Button();
		var controls = new ControlCollection(randomControl);
		handler.Init(controls);
	}

	[Fact]
	public void Should_Handle_Empty_InputUpdate()
	{
		var handler = new PauseHandler();
		var inputEvent = new InputEvent();

		handler.Update(inputEvent);

		Assert.False(handler.IsPaused);
	}

	[Fact]
	public void Should_Pause()
	{
		var handler = new PauseHandler();

		var randomControl = new Button();
		var controls = new ControlCollection(randomControl);
		handler.Init(controls);

		var pauseEvent = new InputEvent()
		{
			KeyboardEvent = new KeyboardEvent(new KeyEventArgs(Keys.P), KeyEventType.KeyUp)
		};
		handler.Update(pauseEvent);
		Assert.True(handler.IsPaused);

		var unPauseEvent = new InputEvent()
		{
			KeyboardEvent = new KeyboardEvent(new KeyEventArgs(Keys.P), KeyEventType.KeyUp)
		};
		handler.Update(unPauseEvent);
		Assert.False(handler.IsPaused);
	}
}
