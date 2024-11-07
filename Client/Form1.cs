using Client.HubClients;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Shared;

namespace Client;

public partial class Form1 : Form, IMessageFilter
{
    private IHubClient? _mainHubClient;
    private ClientUpdateHandler _updateHandler;
	private List<Obstacle> _obstacles = [];
    private ShootingHandler _shootingHandler = new ShootingHandler();

	public Form1()
	{
		InitializeComponent();
		Application.AddMessageFilter(this);
		this.Paint += new PaintEventHandler(OnPaint);

        InitializeWeapon();
	}

	private async void Form1_Load(object sender, EventArgs e)
	{
		_mainHubClient = await MainHubClient.GetClientAsync();

		_updateHandler = new(this, _mainHubClient);
		KeyDown += _updateHandler.OnKeyDown;
		KeyUp += _updateHandler.OnKeyUp;

		_mainHubClient.Connection.On<string?, string?>("ReceiveGameUpdate", (playerUpdateResponseJson, enemiesUpdateResponseJson) =>
		{
			Invoke(() => OnReceiveGameUpdate(playerUpdateResponseJson, enemiesUpdateResponseJson));
		});
		_mainHubClient.Connection.On<int>("ReceivePersonalId", id =>
		{
			Globals.PersonalID = id;
			Invoke(() => label1.Text = $"Personal id: {id}");
		});
		_mainHubClient.Connection.On<string>("ReceiveObstaclesUpdate", obstaclesJson =>
		{
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

    private void InitializeWeapon()
    {
        Weapon initialWeapon = new Pistol();
        _shootingHandler.SetWeapon(initialWeapon);
    }

	private void OnReceiveGameUpdate(string PlayersJson, string EnemiesJson)
	{
		var players = JsonConvert.DeserializeObject<List<Player>>(PlayersJson);
		players.ForEach(Render);

		var enemies = DeserializeEnemies.DeserializeEnemy(EnemiesJson);
		enemies.ForEach(Render);
	}

	private void Render(IRenderable renderable)
	{
		PictureBox? existingPicture = null;
		foreach (var picture in this.Controls.OfType<PictureBox>())
		{
			if (picture.Tag is int numericTag && numericTag == renderable.Id)
			{
				existingPicture = picture;
			}
		}

		if (existingPicture == null)
		{
			existingPicture = new();

			Update(existingPicture, renderable);
			this.Controls.Add(existingPicture);
		}
		else
		{
			Update(existingPicture, renderable);
		}
	}

	private void Update(PictureBox picture, IRenderable renderable)
	{
		picture.Tag = renderable.Id;
		picture.Left = renderable.X;
		picture.Top = renderable.Y;
		picture.SizeMode = PictureBoxSizeMode.AutoSize;
		picture.Image = (Bitmap)Sprites.ResourceManager.GetObject(renderable.Image);
	}

	public bool PreFilterMessage(ref Message m)
	{
		_updateHandler?.OnWindowsMessage(ref m);
		return false;
	}

	private void label1_Click(object sender, EventArgs e) { }

	/// <summary>
	/// Repeats every set GameTimer interval (the interval is set from the Form1 design screen)
	/// Mainly used for sending updates to the server about player actions
	/// </summary>
	private void GameTimer_Tick(object sender, EventArgs e) =>
		_ = _updateHandler?.UpdateAsync();
}
