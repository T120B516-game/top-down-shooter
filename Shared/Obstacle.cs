using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared
{
    public abstract class Obstacle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Type { get; set; }

        public abstract void Draw(Graphics g);
    }

    public class Penetratable : Obstacle
    {
        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Green, X, Y, Width, Height);
        }
    }

    public class Unpenetratable : Obstacle
    {
        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Red, X, Y, Width, Height);
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
        private Penetratable _obstacle = new Penetratable();

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
        private Unpenetratable _obstacle = new Unpenetratable();

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
        public IObstacleBuilder CreatePenetratableBuilder()
        {
            return new PenetratableObstacleBuilder();
        }

        public IObstacleBuilder CreateUnpenetratableBuilder()
        {
            return new UnpenetratableObstacleBuilder();
        }
    }

    // JSON converter for Obstacles
    public class ObstacleConverter : JsonConverter<Obstacle>
    {
        private readonly ObstacleFactory _factory = new ObstacleFactory();

        public override Obstacle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonObject = jsonDocument.RootElement;

            var type = jsonObject.GetProperty("Type").GetString();
            IObstacleBuilder builder;

            if (type == "Penetratable")
            {
                builder = _factory.CreatePenetratableBuilder();
            }
            else if (type == "Unpenetratable")
            {
                builder = _factory.CreateUnpenetratableBuilder();
            }
            else
            {
                throw new ArgumentException("Unknown obstacle type");
            }

            int x = jsonObject.GetProperty("X").GetInt32();
            int y = jsonObject.GetProperty("Y").GetInt32();
            int width = jsonObject.GetProperty("Width").GetInt32();
            int height = jsonObject.GetProperty("Height").GetInt32();

            return builder.SetPosition(x, y)
                          .SetSize(width, height)
                          .Build();
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
        public static List<Obstacle> DeserializeObstacles(string json)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new ObstacleConverter() }
            };
            return JsonSerializer.Deserialize<List<Obstacle>>(json, options);
        }
    }
}
