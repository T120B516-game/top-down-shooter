using Client.Entities;
using Client.HubClients;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace Client;

public partial class Form1 : Form
{
	private IHubClient _mainHubClient;
	bool goLeft, goRight, goUp, goDown;


	public Form1()
	{
		InitializeComponent();
		this.KeyDown += new KeyEventHandler(OnKeyDown);
		this.KeyUp += new KeyEventHandler(OnKeyUp);
	}

	private async void Form1_Load(object sender, EventArgs e)
	{
		_mainHubClient = await MainHubClient.GetClientAsync();


		_mainHubClient.Connection.On<string?>("ReceiveGameUpdate", playersJson =>       // Receive regular game data updates
		{
			List<Player> Players = JsonConvert.DeserializeObject<List<Player>>(playersJson);
			Invoke(() => UpdateView(Players));
		});

		_mainHubClient.Connection.On<int>("ReceivePersonalId", id =>        // Receive personal id of a player for identification (should be called only once after sedning "CreatePlayer" message to server)
		{
			Globals.PersonalID = id;
			label1.Text = $"Personal id: {id}";
		});

		await _mainHubClient.Connection.SendAsync("CreatePlayer");  // Ask server to create a player and send over am personal id
	}

	/// <summary>
	/// Updates entities display on the form
	/// </summary>
	private void UpdateView(List<Player> entities)
	{
		foreach (var entity in entities)        // Iterates trough each entity and either finds it in the form and updates its position, or if does not exist yet - creates it
		{
			PictureBox entityPicture = null;
			foreach (Control obj in this.Controls)  // Iterates trough each Control in the scene and checks if its the player PictureBox
			{
				if (obj is PictureBox && (int)obj.Tag == entity.Id)
				{
					entityPicture = (PictureBox)obj;
					entityPicture.Left = entity.Horizontal;
					entityPicture.Top = entity.Vertical;
					entityPicture.SizeMode = PictureBoxSizeMode.AutoSize;
					entityPicture.Image = (Bitmap)Sprites.ResourceManager.GetObject(entity.Image);
					break;
				}
			}

			if (entityPicture is null)
			{
				entityPicture = new PictureBox();
				entityPicture.Tag = entity.Id;
				entityPicture.Left = entity.Horizontal;
				entityPicture.Top = entity.Vertical;
				entityPicture.Image = (Bitmap)Sprites.ResourceManager.GetObject(entity.Image);
				entityPicture.SizeMode = PictureBoxSizeMode.AutoSize;
				this.Controls.Add(entityPicture);
			}
		}
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Left)
			goLeft = true;

		if (e.KeyCode == Keys.Right)
			goRight = true;

		if (e.KeyCode == Keys.Up)
			goUp = true;

		if (e.KeyCode == Keys.Down)
			goDown = true;
	}

	private void OnKeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Left)
			goLeft = false;

		if (e.KeyCode == Keys.Right)
			goRight = false;

		if (e.KeyCode == Keys.Up)
			goUp = false;

		if (e.KeyCode == Keys.Down)
			goDown = false;
	}

	private void label1_Click(object sender, EventArgs e) { }


	/// <summary>
	/// Repeats every set GameTimer interval (the interval is set from the Form1 design screen)
	/// Mainly used for sending updates to the server about player actions
	/// </summary>
	private void GameTimer_Tick(object sender, EventArgs e)
	{
		if (goUp)
			_mainHubClient.Connection.SendAsync("movePlayer", 1, Globals.PersonalID);
		if (goRight)
			_mainHubClient.Connection.SendAsync("movePlayer", 2, Globals.PersonalID);
		if (goDown)
			_mainHubClient.Connection.SendAsync("movePlayer", 3, Globals.PersonalID);
		if (goLeft)
			_mainHubClient.Connection.SendAsync("movePlayer", 4, Globals.PersonalID);
	}
}
