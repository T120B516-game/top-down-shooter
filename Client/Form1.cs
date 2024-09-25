using Client.HubClients;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client;

public partial class Form1 : Form
{
	private IHubClient _mainHubClient;

	public Form1()
	{	
		InitializeComponent();
	}

	private async void Form1_Load(object sender, EventArgs e)
	{
		_mainHubClient = await MainHubClient.GetClientAsync();

		_mainHubClient.Connection.On<int>("ReceiveConnectedUsersCount", count =>
		{
			Invoke(() => label1.Text = $"Connected players: {count}");
		});

		await _mainHubClient.Connection.SendAsync("RequestConnectedUsersCount");
	}

	private void label1_Click(object sender, EventArgs e)
	{

	}
}
