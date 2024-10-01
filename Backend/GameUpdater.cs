using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Backend.Entities;
using Backend.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Backend
{
	/// <summary>
	/// Main update cycle of the game
	/// </summary>
	static public class GameUpdater
	{
		static private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(20);

		static private Timer _timer;

		static private IHubCallerClients? Clients = null;

		/// <summary>
		/// Once this function is called, BroadcastGameUpdate starts getting invoked repeatedly (every _updateInterval), broadcasting the game data to all clients
		/// </summary>
		static public void StartGameUpdater(IHubCallerClients hubContext)
		{
			if(Clients is not null) return;

			Clients = hubContext;

			_timer = new Timer(BroadcastGameUpdate, null, _updateInterval, _updateInterval);
		}

		static async void BroadcastGameUpdate(object state)
		{
			var playersJson = JsonConvert.SerializeObject(GameData.Players);

			await Clients.All.SendAsync("ReceiveGameUpdate", playersJson);
		}
	}
}
