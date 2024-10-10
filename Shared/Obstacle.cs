using System;
using System.Collections.Generic;
using System.Drawing;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
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

    public class ObstacleFactory
    {
        public Obstacle CreateObstacle(string type)
        {
            if (type == "Penetratable")
            {
                return new Penetratable();
            }
            else if (type == "Unpenetratable")
            {
                return new Unpenetratable();
            }
            else
            {
                throw new ArgumentException("Unknown obstacle type");
            }
        }
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


    public class ObstacleConverter : JsonConverter<Obstacle>
    {
        private readonly ObstacleFactory _factory = new ObstacleFactory();

        public override Obstacle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonObject = jsonDocument.RootElement;

            var type = jsonObject.GetProperty("Type").GetString();

            Obstacle obstacle = _factory.CreateObstacle(type);

            foreach (var property in jsonObject.EnumerateObject())
            {
                switch (property.Name)
                {
                    case "X":
                        obstacle.X = property.Value.GetInt32();
                        break;
                    case "Y":
                        obstacle.Y = property.Value.GetInt32();
                        break;
                    case "Width":
                        obstacle.Width = property.Value.GetInt32();
                        break;
                    case "Height":
                        obstacle.Height = property.Value.GetInt32();
                        break;
                }
            }

            obstacle.Type = type; 

            return obstacle;
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
