using static System.Windows.Forms.Control;

namespace Client.Observer;

public class CrosshairRenderer : IInputObserver, IUpdateable
{
	private Point _nextLocation = new(0, 0);
	private PictureBox _crosshair;

	public void Init(ControlCollection controls)
	{
		_crosshair = new PictureBox()
		{
			Tag = "crosshair",
			Left = 0,
			Top = 0,
			Width = 24,
			Height = 24,
			Image = (Bitmap)Sprites.ResourceManager.GetObject("crosshair"),
		};

		Cursor.Hide();
		controls.Add(_crosshair);
	}

	public void Update(InputEvent input)
	{
		var mouseEvent = input.MouseEvent;
		if (mouseEvent == null || mouseEvent.Type != MouseEventType.Move)
		{
			return;
		};

		var x = mouseEvent.Location.X - (_crosshair.Width / 2);
		var y = mouseEvent.Location.Y - (_crosshair.Height / 2);
		_nextLocation = new Point(x, y);
	}

	public void Update()
	{
		_crosshair.Location = _nextLocation;
		_crosshair.Invalidate();
	}
}
