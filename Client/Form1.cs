using Client.HubClients;
using Client.Mediator;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Shared;

namespace Client;

public partial class Form1 : Form, IMessageFilter
{
	public bool IsReady { get; set; }

	public ClientUpdateHandler UpdateHandler { get; private set; }
	private IHubClient? _mainHubClient;
	private List<Obstacle> _obstacles = [];
	private ShootingHandler _shootingHandler = new ShootingHandler();

	private AssetManager _assetManager = new();
	private NetworkComponent _networkComponent;
	private GameMediator _mediator;

	public Form1()
	{
		Globals.Form = this;

		InitializeComponent();
		Application.AddMessageFilter(this);
		this.Paint += new PaintEventHandler(OnPaint);
		this.KeyDown += KeyIsDown;

		InitializeWeapon();
	}

	public async void Form1_Load(object sender, EventArgs e)
	{
		_mainHubClient = await MainHubClient.GetClientAsync();

		_assetManager = new AssetManager();
		_networkComponent = new NetworkComponent();
		_mediator = new GameMediator(_assetManager, _networkComponent);

		UpdateHandler = new(this, new NetworkHandler(_mainHubClient.Connection));
		KeyDown += UpdateHandler.OnKeyDown;
		KeyUp += UpdateHandler.OnKeyUp;

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
		_mainHubClient.Connection.On<int>("RemoveEnemy", enemyId =>
		{
			Invoke(() => _networkComponent.OnRemoveEnemy(enemyId));
		});
		_mainHubClient.Connection.On<int, int, int, string>("SpawnBullet", (playerId, x, y, direction) =>
		{
			Invoke(() => _networkComponent.OnSpawnBullet(playerId, x, y, direction));
		});

		await _mainHubClient.Connection.SendAsync("CreatePlayer");

		IsReady = true;
	}

	private void OnPaint(object sender, PaintEventArgs e)
	{
		Graphics g = e.Graphics;

		foreach (var obstacle in _obstacles)
		{
			obstacle.Draw(g, Globals.ColliderMap);
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

		Player? thisPlayer = players.Where(p => p.Id == Globals.PersonalID).FirstOrDefault();
		if (thisPlayer != null)
			Globals.ThisPlayer.UpdatePlayer(thisPlayer);

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
		UpdateHandler?.OnWindowsMessage(ref m);
		return false;
	}

	private void label1_Click(object sender, EventArgs e) { }

	private void KeyIsDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Space)
		{
			new Bullet(this, Globals.ThisPlayer, UpdateHandler);
			UpdateHandler.HandleBulletFired(Globals.PersonalID);
		}
	}

	/// <summary>
	/// Repeats every set GameTimer interval (the interval is set from the Form1 design screen)
	/// Mainly used for sending updates to the server about player actions
	/// </summary>
	private void GameTimer_Tick(object sender, EventArgs e) =>
		_ = UpdateHandler?.UpdateAsync();
}
