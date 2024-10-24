using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared
{
	public abstract class Enemy : IClonable<Enemy>
	{
		public int Id { get; set; }
		public int X {  get; set; }
		public int Y { get; set; }
		public string Image { get; set; }
		public int Health { get; set; }
		public string Type { get; set; }

		//public IMovementBehaviour MovementBehaviour { get; set;}

		protected Dictionary<string, string> ImagesDictionary;

		public Enemy(int _X, int _Y, int _health, int id)
		{
			this.X = _X;
			this.Y = _Y;
			this.Health = _health;
			Id = id;
		}

		public abstract void DoNextAction();
		public abstract void SetMovementBehaviour();

		virtual public Enemy DeepClone()
		{
			Enemy enemyClone;
			switch (this.Type)
			{
				case "mobile_explosive":
					enemyClone = new MobileExplosiveEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				case "mobile_shooting":
					enemyClone = new MobileShootingEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				case "mobile_meele":
					enemyClone = new MobileMeeleEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				case "stationary_explosive":
					enemyClone = new StationaryExplosiveEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				case "stationary_shooting":
					enemyClone = new StationaryShootingEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				case "statioanry_meele":
					enemyClone = new StationaryMeeleEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				default:
					throw new ArgumentException("Unknown enemy type");
			}
			enemyClone.Image = this.Image;
			// IMovementBehaviour MovementBehaviour = this.MovementBehaviour;
			return enemyClone;
		}

		virtual public Enemy ShallowClone()
		{
			Enemy enemyClone;
			switch (this.Type)
			{
				case "mobile_explosive":
					enemyClone = new MobileExplosiveEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				case "mobile_shooting":
					enemyClone = new MobileShootingEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				case "mobile_meele":
					enemyClone = new MobileMeeleEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				case "stationary_explosive":
					enemyClone = new StationaryExplosiveEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				case "stationary_shooting":
					enemyClone = new StationaryShootingEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				case "statioanry_meele":
					enemyClone = new StationaryMeeleEnemy(this.X, this.Y, this.Health, this.Id);
					break;
				default:
					throw new ArgumentException("Unknown enemy type");
			}
			enemyClone.Image = this.Image;
			return enemyClone;
		}
	}

	public class MobileExplosiveEnemy : Enemy
	{
		public int Speed { get; set; }
		public MobileExplosiveEnemy(int _X, int _Y, int _Health, int _Id) : base(_X, _Y, _Health, _Id)
		{
			this.Type = "mobile_explosive";
			this.ImagesDictionary = new Dictionary<string, string>()
			{
				{ "down", "MobileExplosiveDown" },
				{ "up", "MobileExplosiveUp"},
				{ "right", "MobileExplosiveRight"},
				{ "left", "MobileExplosiveLeft"}
			};
			this.Image = ImagesDictionary["up"];
			this.Speed = 20;
		}

		public override void DoNextAction()
		{
			throw new NotImplementedException();
		}

		public override void SetMovementBehaviour()
		{
			throw new NotImplementedException();
		}
	}

	public class MobileShootingEnemy : Enemy
	{
		public int Speed { get; set; }
		public MobileShootingEnemy(int _X, int _Y, int _Health, int _Id) : base(_X, _Y, _Health, _Id)
		{
			this.Type = "mobile_shooting";
			this.ImagesDictionary = new Dictionary<string, string>()
			{
				{ "down", "MobileShootingDown" },
				{ "up", "MobileShootingUp"},
				{ "right", "MobileShootinRight"},
				{ "left", "MobileShootingLeft"}
			};
			this.Image = ImagesDictionary["up"];
			this.Speed = 20;
		}

		public override void DoNextAction()
		{
			throw new NotImplementedException();
		}

		public override void SetMovementBehaviour()
		{
			throw new NotImplementedException();
		}
	}

	public class MobileMeeleEnemy : Enemy
	{
		public int Speed { get; set; }
		public MobileMeeleEnemy(int _X, int _Y, int _Health, int _Id) : base(_X, _Y, _Health, _Id)
		{
			this.Type = "mobile_meele";
			this.ImagesDictionary = new Dictionary<string, string>()
			{
				{ "down", "MobileMeeleDown" },
				{ "up", "MobileMeeleUp"},
				{ "right", "MobileMeeleRight"},
				{ "left", "MobileMeeleLeft"}
			};
			this.Image = ImagesDictionary["up"];
			this.Speed = 20;
		}

		public override void DoNextAction()
		{
			throw new NotImplementedException();
		}

		public override void SetMovementBehaviour()
		{
			throw new NotImplementedException();
		}
	}

	public class StationaryExplosiveEnemy : Enemy
	{
		public StationaryExplosiveEnemy(int _X, int _Y, int _Health, int _Id) : base(_X, _Y, _Health, _Id) 
		{
			this.Type = "stationary_explosive";
			this.ImagesDictionary = new Dictionary<string, string>()
			{
				{ "down", "StationaryExplosiveDown" },
				{ "up", "StationaryExplosiveUp"},
				{ "right", "StationaryExplosiveRight"},
				{ "left", "StationaryExplosiveLeft"}
			};
			this.Image = ImagesDictionary["up"];
		}

		public override void DoNextAction()
		{
			throw new NotImplementedException();
		}

		public override void SetMovementBehaviour()
		{
			throw new NotImplementedException();
		}

	}

	public class StationaryShootingEnemy : Enemy
	{
		public StationaryShootingEnemy(int _X, int _Y, int _Health, int _Id) : base(_X, _Y, _Health, _Id) 
		{
			this.Type = "stationary_shooting";
			this.ImagesDictionary = new Dictionary<string, string>()
			{
				{ "down", "StationaryShootingDown" },
				{ "up", "StationaryShootingUp"},
				{ "right", "StationaryShootingRight"},
				{ "left", "StationaryShootingLeft"}
			};
			this.Image = ImagesDictionary["up"];
		}


		public override void DoNextAction()
		{
			throw new NotImplementedException();
		}

		public override void SetMovementBehaviour()
		{
			throw new NotImplementedException();
		}
	}

	public class StationaryMeeleEnemy : Enemy
	{
		public StationaryMeeleEnemy(int _X, int _Y, int _Health, int _Id) : base(_X, _Y, _Health, _Id) 
		{
			this.Type = "meele_explosive";
			this.ImagesDictionary = new Dictionary<string, string>()
			{
				{ "down", "StationaryMeeleDown" },
				{ "up", "StationaryMeeleUp"},
				{ "right", "StationaryMeeleRight"},
				{ "left", "StationaryMeeleLeft"}
			};
			this.Image = ImagesDictionary["up"];
		}

		public override void DoNextAction()
		{
			throw new NotImplementedException();
		}

		public override void SetMovementBehaviour()
		{
			throw new NotImplementedException();
		}
	}

	public class EnemyConverter : JsonConverter<Enemy>
	{
		public override Enemy? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var jsonDocument = JsonDocument.ParseValue(ref reader);
			var jsonObject = jsonDocument.RootElement;

			int x = jsonObject.GetProperty("X").GetInt32();
			int y = jsonObject.GetProperty("Y").GetInt32();
			int health = jsonObject.GetProperty("Health").GetInt32();
			var type = jsonObject.GetProperty("Type").GetString();
			int id = jsonObject.GetProperty("Id").GetInt32();
			Enemy enemy;

			switch (type)
			{
				case "mobile_explosive":
					enemy = new MobileExplosiveEnemy(x, y, health, id);
					break;
				case "mobile_shooting":
					enemy = new MobileShootingEnemy(x, y, health, id);
					break;
				case "mobile_meele":
					enemy = new MobileMeeleEnemy(x, y, health, id);
					break;
				case "stationary_explosive":
					enemy = new StationaryExplosiveEnemy(x, y, health, id);
					break;
				case "stationary_shooting":
					enemy = new StationaryShootingEnemy(x, y, health, id);
					break;
				case "statioanry_meele":
					enemy = new StationaryMeeleEnemy(x, y, health, id);
					break;
				default:
					throw new ArgumentException("Unknown enemy type");
			}

			return enemy;
		}

		public override void Write(Utf8JsonWriter writer, Enemy value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("X", value.X);
			writer.WriteNumber("Y", value.Y);
			writer.WriteNumber("Health", value.Health);
			writer.WriteString("Type", value.Type);
			writer.WriteNumber("Id", value.Id);
			writer.WriteEndObject();
		}
	}

	public class DeserializeEnemies
	{
		public static List<Enemy> DeserializeEnemy(string json)
		{
			var options = new JsonSerializerOptions
			{
				Converters = { new EnemyConverter() }
			};
			return JsonSerializer.Deserialize<List<Enemy>>(json, options);
		}
	}
}