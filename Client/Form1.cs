using Client.HubClients;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Shared;

namespace Client;

public partial class Form1 : Form
{
	private IHubClient? _mainHubClient;
	private MovementHandler _movementHandler = new();
    private List<Obstacle> _obstacles = new List<Obstacle>();

    public Form1()
	{
		InitializeComponent();
		this.KeyDown += new KeyEventHandler(OnKeyDown);
		this.KeyUp += new KeyEventHandler(OnKeyUp);
        this.Paint += new PaintEventHandler(OnPaint);
    }

	private async void Form1_Load(object sender, EventArgs e)
	{
		_mainHubClient = await MainHubClient.GetClientAsync();

		_mainHubClient.Connection.On<string?>("ReceiveGameUpdate", updateResponseJson =>
		{
			Invoke(() => OnReceiveGameUpdate(updateResponseJson));
		});
		_mainHubClient.Connection.On<int>("ReceivePersonalId", id =>
		{
			Globals.PersonalID = id;
			label1.Text = $"Personal id: {id}";
		});
		_mainHubClient.Connection.On<string>("ReceiveObstaclesUpdate", obstaclesJson =>
		{
			_obstacles = null;
            _obstacles = DeserializerObstacles.DeserializeObstacles(obstaclesJson);
            Invalidate();

		});


		await _mainHubClient.Connection.SendAsync("CreatePlayer");
	}

    private void OnPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        foreach (var obstacle in _obstacles)
        {
            obstacle.Draw(g);
        }
    }

    private void OnReceiveGameUpdate(string updateResponseJson)
	{
		var entities = JsonConvert.DeserializeObject<List<Player>>(updateResponseJson);

		foreach (var entity in entities)
		{
			PictureBox entityPicture = null;

			foreach (Control control in this.Controls)
			{
				if (control is PictureBox box && (int)control.Tag == entity.Id)
				{
					entityPicture = box;
					entityPicture.Left = entity.X;
					entityPicture.Top = entity.Y;
					entityPicture.SizeMode = PictureBoxSizeMode.AutoSize;
					entityPicture.Image = (Bitmap)Sprites.ResourceManager.GetObject(entity.Image);
					break;
				}
			}

			if (entityPicture == null)
			{
				entityPicture = new PictureBox
				{
					Tag = entity.Id,
					Left = entity.X,
					Top = entity.Y,
					Image = (Bitmap)Sprites.ResourceManager.GetObject(entity.Image),
					SizeMode = PictureBoxSizeMode.AutoSize
				};
				this.Controls.Add(entityPicture);
			}
		}
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
		_movementHandler.ConsumeKeyEvent(e, KeyEventType.KeyDown);
	}

	private void OnKeyUp(object sender, KeyEventArgs e)
	{
		_movementHandler.ConsumeKeyEvent(e, KeyEventType.KeyUp);
	}

	private void label1_Click(object sender, EventArgs e) { }


	/// <summary>
	/// Repeats every set GameTimer interval (the interval is set from the Form1 design screen)
	/// Mainly used for sending updates to the server about player actions
	/// </summary>
	private void GameTimer_Tick(object sender, EventArgs e)
	{
		if (_mainHubClient == null)
		{
			return;
		}

		_ = _movementHandler.SendAsync(_mainHubClient.Connection);
	}
}
