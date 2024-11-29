using Client.HubClients;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Shared;

namespace Client;

public partial class Form1 : Form, IMessageFilter
{
    private IHubClient? _mainHubClient;
    private ClientUpdateHandler _updateHandler;
    private readonly ImageFactory _imageFactory = new();
    private ObstacleRepository _obstacleRepository;
    private List<Obstacle> _obstacles = new();
    private ShootingHandler _shootingHandler = new ShootingHandler();

    public Form1()
    {
        InitializeComponent();
        Application.AddMessageFilter(this);
        this.Paint += new PaintEventHandler(OnPaint);

        InitializeWeapon();
        InitializeObstacles();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        _mainHubClient = await MainHubClient.GetClientAsync();

        _updateHandler = new(this, new NetworkHandler(_mainHubClient.Connection));
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
            _obstacles = JsonConvert.DeserializeObject<List<Obstacle>>(obstaclesJson);
            Invalidate();

        });

        await _mainHubClient.Connection.SendAsync("CreateObstacles", JsonConvert.SerializeObject(_obstacles));

        await _mainHubClient.Connection.SendAsync("CreatePlayer");
    }

    private void InitializeObstacles()
    {
        _obstacleRepository = new ObstacleRepository();
        var obstacles = _obstacleRepository.CreateDefaultObstacles();

        // Add them to the local list for rendering
        _obstacles.AddRange(obstacles);
    }

    /// <summary>
    /// Method to render obstacles on the form.
    /// </summary>
    private void RenderObstacles()
    {
        // Clear existing controls related to obstacles
        var obstacleControls = this.Controls.OfType<PictureBox>()
            .Where(pb => pb.Tag is Obstacle)
            .ToList();

        foreach (var control in obstacleControls)
        {
            this.Controls.Remove(control);
            control.Dispose();
        }

        // Add PictureBox for each obstacle
        foreach (var obstacle in _obstacles)
        {
            var pictureBox = new PictureBox
            {
                Tag = obstacle,
                Left = obstacle.X,
                Top = obstacle.Y,
                Width = obstacle.Width,
                Height = obstacle.Height,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Load the image for the obstacle
            var image = _imageFactory.Get(obstacle.Image) as Bitmap;
            if (image != null)
            {
                pictureBox.Image = image;
            }
            else
            {
                // Fallback to default image if not found
                pictureBox.Image = new Bitmap("Resources/default.png");
            }

            this.Controls.Add(pictureBox);
        }

        // Force the form to redraw
        Invalidate();
    }
    private void OnPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        foreach (var obstacle in _obstacles)
        {
            obstacle.Draw(g, _imageFactory);
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

        var image = Sprites.ResourceManager.GetObject(renderable.Image) as Bitmap;
        if (image != null)
        {
            picture.Image = image;
        }
        else
        {
            picture.Image = new Bitmap($"Resources/up.png"); // Handle missing images gracefully.
        }
    }

    public bool PreFilterMessage(ref Message m)
    {
        _updateHandler?.OnWindowsMessage(ref m);
        return false;
    }

    private void label1_Click(object sender, EventArgs e) { }

    private void GameTimer_Tick(object sender, EventArgs e) =>
        _ = _updateHandler?.UpdateAsync();
}
