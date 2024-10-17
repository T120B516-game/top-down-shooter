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

		_mainHubClient.Connection.On<string?, string?>("ReceiveGameUpdate", (playerUpdateResponseJson, enemiesUpdateResponseJson) =>
		{
			Invoke(() => OnReceiveGameUpdate(playerUpdateResponseJson, enemiesUpdateResponseJson));
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

    private void OnReceiveGameUpdate(string PlayersJson, string EnemiesJson)
	{
		var players = JsonConvert.DeserializeObject<List<Player>>(PlayersJson);

		foreach (var player in players)
		{
			PictureBox playerPicture = null;

			foreach (Control control in this.Controls)
			{
				if (control is PictureBox box && (int)control.Tag == player.Id)
				{
					playerPicture = box;
					playerPicture.Left = player.X;
					playerPicture.Top = player.Y;
					playerPicture.SizeMode = PictureBoxSizeMode.AutoSize;
					playerPicture.Image = (Bitmap)Sprites.ResourceManager.GetObject(player.Image);
					break;
				}
			}

			if (playerPicture == null)
			{
				playerPicture = new PictureBox
				{
					Tag = player.Id,
					Left = player.X,
					Top = player.Y,
					Image = (Bitmap)Sprites.ResourceManager.GetObject(player.Image),
					SizeMode = PictureBoxSizeMode.AutoSize
				};
				this.Controls.Add(playerPicture);
			}
		}

		// Will optimize overlaping code in the future... maybe
		var enemies = DeserializeEnemies.DeserializeEnemy(EnemiesJson);
		foreach (var enemy in enemies)
		{
			PictureBox enemyPicture = null;

			foreach (Control control in this.Controls)
			{
				if (control is PictureBox box && (int)control.Tag == enemy.Id)
				{
					enemyPicture = box;
					enemyPicture.Left = enemy.X;
					enemyPicture.Top = enemy.Y;
					enemyPicture.SizeMode = PictureBoxSizeMode.AutoSize;
					enemyPicture.Image = (Bitmap)Sprites.ResourceManager.GetObject(enemy.Image);
					break;
				}
			}

			if (enemyPicture == null)
			{
				enemyPicture = new PictureBox
				{
					Tag = enemy.Id,
					Left = enemy.X,
					Top = enemy.Y,
					Image = (Bitmap)Sprites.ResourceManager.GetObject(enemy.Image),
					SizeMode = PictureBoxSizeMode.AutoSize
				};
				this.Controls.Add(enemyPicture);
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
