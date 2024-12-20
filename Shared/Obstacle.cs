﻿using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared;

public abstract class Obstacle : IVisitable
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Type { get; set; }

    public abstract void Draw(Graphics g, int[,] ColliderMap);

    public void Accept(IInteractionVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class Penetratable : Obstacle
{
    public override void Draw(Graphics g, int[,] ColliderMap)
    {
        g.FillRectangle(Brushes.Green, X, Y, Width, Height);

        for (int x = this.X; x < this.X + this.Width; x++)
        {
	        for (int y = this.Y; y < this.Y + this.Height; y++)
	        {
		        ColliderMap[x, y] = 1;
	        }
        }
	}
}

public class Unpenetratable : Obstacle
{
    public override void Draw(Graphics g, int[,] ColliderMap)
    {
        g.FillRectangle(Brushes.Red, X, Y, Width, Height);

        for (int x = this.X; x < this.X + this.Width; x++)
        {
	        for (int y = this.Y; y < this.Y + this.Height; y++)
	        {
		        ColliderMap[x, y] = 2;
	        }
        }
	}
}

public interface IObstacleBuilder
{
    IObstacleBuilder SetPosition(int x, int y);
    IObstacleBuilder SetSize(int width, int height);
    Obstacle Build();
}

public class PenetratableObstacleBuilder : IObstacleBuilder
{
    private readonly Penetratable _obstacle = new();

    public IObstacleBuilder SetPosition(int x, int y)
    {
        _obstacle.X = x;
        _obstacle.Y = y;
        return this;
    }

    public IObstacleBuilder SetSize(int width, int height)
    {
        _obstacle.Width = width;
        _obstacle.Height = height;
        return this;
    }

    public Obstacle Build()
    {
        _obstacle.Type = "Penetratable";
        return _obstacle;
    }
}

public class UnpenetratableObstacleBuilder : IObstacleBuilder
{
    private readonly Unpenetratable _obstacle = new();

    public IObstacleBuilder SetPosition(int x, int y)
    {
        _obstacle.X = x;
        _obstacle.Y = y;
        return this;
    }

    public IObstacleBuilder SetSize(int width, int height)
    {
        _obstacle.Width = width;
        _obstacle.Height = height;
        return this;
    }

    public Obstacle Build()
    {
        _obstacle.Type = "Unpenetratable";
        return _obstacle;
    }
}

public class ObstacleFactory
{
    public Obstacle CreatePenetratable(int X, int Y, int sizeX, int sizeY)
    {
        var builder = new PenetratableObstacleBuilder();
		return builder.SetPosition(X, Y)
			    .SetSize(sizeX, sizeY)
			    .Build();
	}

	public Obstacle CreateUnpenetratable(int X, int Y, int sizeX, int sizeY)
    {
		var builder = new UnpenetratableObstacleBuilder();
		return builder.SetPosition(X, Y)
				.SetSize(sizeX, sizeY)
				.Build();
    }
}

// JSON converter for Obstacles
public class ObstacleConverter : JsonConverter<Obstacle>
{
    private readonly ObstacleFactory _factory = new();

    public override Obstacle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var jsonObject = jsonDocument.RootElement;

        var type = jsonObject.GetProperty("Type").GetString();

		int x = jsonObject.GetProperty("X").GetInt32();
		int y = jsonObject.GetProperty("Y").GetInt32();
		int width = jsonObject.GetProperty("Width").GetInt32();
		int height = jsonObject.GetProperty("Height").GetInt32();

		if (type == "Penetratable")
        {
            return _factory.CreatePenetratable(x, y, width, height);
        }
        else if (type == "Unpenetratable")
        {
			return _factory.CreateUnpenetratable(x, y, width, height);
		}
        else
        {
            throw new ArgumentException("Unknown obstacle type");
        }
    }

    public override void Write(Utf8JsonWriter writer, Obstacle value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("X", value.X);
        writer.WriteNumber("Y", value.Y);
        writer.WriteNumber("Width", value.Width);
        writer.WriteNumber("Height", value.Height);
        writer.WriteString("Type", value.Type);
        writer.WriteEndObject();
    }
}

public class DeserializerObstacles
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = 
        new()
	    {
	    	Converters = 
            { 
                new ObstacleConverter() 
            }
	    };

	public static List<Obstacle> DeserializeObstacles(string json)
    {
        return JsonSerializer.Deserialize<List<Obstacle>>(json, jsonSerializerOptions);
    }
}
