using Client.Observer;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace Tests.Observer;

public class CrosshairRendererTests
{
	[Fact]
	public void Should_Initialize()
	{
		var crosshair = new CrosshairRenderer();
		
		var randomControl = new Button();
		var controls = new ControlCollection(randomControl);

		crosshair.Init(controls);
	}

	[Fact]
	public void Should_Handle_Empty_InputUpdate()
	{
		var crosshair = new CrosshairRenderer();
		var targetCoords = new Point(0, 0);
		var inputEvent = new InputEvent();

		crosshair.Update(inputEvent);

		Assert.Equal(targetCoords, crosshair.NextLocation);
	}

	[Fact]
	public void Should_Handle_MouseMove_InputUpdate()
	{
		var crosshair = new CrosshairRenderer();
		var targetCoords = new Point(10, 10);
		var inputEvent = new InputEvent()
		{
			MouseEvent = new MouseEvent(targetCoords, MouseEventType.Move)
		};

		crosshair.Update(inputEvent);

		Assert.Equal(targetCoords, crosshair.NextLocation);
	}

	[Fact]
	public void Should_Handle_Update()
	{
		var crosshair = new CrosshairRenderer();

		crosshair.Update();
	}
}
