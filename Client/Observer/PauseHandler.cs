using static System.Windows.Forms.Control;

namespace Client.Observer;

public class PauseHandler : IInputObserver
{
	private PictureBox _pause;

	public bool IsPaused { get; private set; } = false;

	public void Init(ControlCollection controls)
	{
		_pause = new PictureBox()
		{
			Tag = "pause",
			Anchor = AnchorStyles.None,
			SizeMode = PictureBoxSizeMode.AutoSize,
		};

		try
		{
			_pause.Image = (Bitmap)Sprites.ResourceManager.GetObject("pause");
		}
		catch { }

		controls.Add(_pause);

		_pause.Hide();
	}

	public void Update(InputEvent input)
	{
		var keyboardEvent = input.KeyboardEvent;
		if (keyboardEvent == null)
		{
			return;
		}

		if (keyboardEvent.Args.KeyCode == Keys.P && keyboardEvent.Type == KeyEventType.KeyUp)
		{
			IsPaused = !IsPaused;
		}

		if (IsPaused)
		{
			_pause.Show();

			_pause.Location = new Point(
				(_pause.Parent.ClientSize.Width / 2) - (_pause.Width / 2),
				(_pause.Parent.ClientSize.Height / 2) - (_pause.Height / 2));
		}
		else
		{
			_pause.Hide();
		}
	}
}
