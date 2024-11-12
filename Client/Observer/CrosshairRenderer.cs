using static System.Windows.Forms.Control;

namespace Client.Observer;

public class CrosshairRenderer : IInputObserver, IUpdateable
{
	public Point NextLocation { get; private set; } = new(0, 0);
	private PictureBox? _crosshair;

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

		var x = mouseEvent.Location.X - ((_crosshair?.Width).GetValueOrDefault() / 2);
		var y = mouseEvent.Location.Y - ((_crosshair?.Height).GetValueOrDefault() / 2);
		NextLocation = new Point(x, y);
	}

	public void Update()
	{
		if (_crosshair != null)
		{
			_crosshair.Location = NextLocation;
			_crosshair.Invalidate();
		}
	}
}
