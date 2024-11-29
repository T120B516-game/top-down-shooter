using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared;

public abstract class Obstacle
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Type { get; set; }
    public string Image { get; set; }
    protected Dictionary<string, string> ImagesDictionary;

    public Obstacle(int x, int y, int width, int height, string type)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Type = type;

        // Define image mapping for obstacle types
        ImagesDictionary = new Dictionary<string, string>
        {
            { "rock", "up.png" },
            { "wall", "up.png" },
            { "tree", "up.png" },
        };

        // Use the type to determine the image, defaulting if not found
        Image = ImagesDictionary.ContainsKey(type) ? ImagesDictionary[type] : "default.png";
    }

    public abstract void Draw(Graphics g, IImageProvider imageProvider);
}

public class Penetratable : Obstacle
{
    public Penetratable(int x, int y, int width, int height)
        : base(x, y, width, height, "rock") { }

    public override void Draw(Graphics g, IImageProvider imageProvider)
    {
        // Fetch the image using the provider and draw it
        var bitmap = imageProvider.Get(Image) as Bitmap;
        if (bitmap != null)
        {
            g.DrawImage(bitmap, X, Y, Width, Height);
        }
        else
        {
            // Fallback to a green rectangle if no image is found
            g.FillRectangle(Brushes.Green, X, Y, Width, Height);
        }
    }
}

public class Unpenetratable : Obstacle
{
    public Unpenetratable(int x, int y, int width, int height)
        : base(x, y, width, height, "wall") { }

    public override void Draw(Graphics g, IImageProvider imageProvider)
    {
        // Fetch the image using the provider and draw it
        var bitmap = imageProvider.Get(Image) as Bitmap;
        if (bitmap != null)
        {
            g.DrawImage(bitmap, X, Y, Width, Height);
        }
        else
        {
            // Fallback to a red rectangle if no image is found
            g.FillRectangle(Brushes.Red, X, Y, Width, Height);
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
    private readonly Penetratable _obstacle;

    public PenetratableObstacleBuilder(int x, int y, int width, int height)
    {
        _obstacle = new Penetratable(x, y, width, height);
    }

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
        return _obstacle;
    }
}

public class UnpenetratableObstacleBuilder : IObstacleBuilder
{
    private readonly Unpenetratable _obstacle;

    public UnpenetratableObstacleBuilder(int x, int y, int width, int height)
    {
        _obstacle = new Unpenetratable(x, y, width, height);
    }

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
        return _obstacle;
    }
}

public class ObstacleFactory
{
    public Obstacle CreatePenetratable(int x, int y, int width, int height)
    {
        var builder = new PenetratableObstacleBuilder(x, y, width, height);
        return builder.Build();
    }

    public Obstacle CreateUnpenetratable(int x, int y, int width, int height)
    {
        var builder = new UnpenetratableObstacleBuilder(x, y, width, height);
        return builder.Build();
    }
}

public class ObstacleConverter : JsonConverter<Obstacle>
{
    private readonly ObstacleFactory _factory;

    public ObstacleConverter(ObstacleFactory factory)
    {
        _factory = factory;
    }

    public override Obstacle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var jsonObject = jsonDocument.RootElement;

        string type = jsonObject.GetProperty("Type").GetString();
        int x = jsonObject.GetProperty("X").GetInt32();
        int y = jsonObject.GetProperty("Y").GetInt32();
        int width = jsonObject.GetProperty("Width").GetInt32();
        int height = jsonObject.GetProperty("Height").GetInt32();

        if (type == "rock")
        {
            return _factory.CreatePenetratable(x, y, width, height);
        }
        else if (type == "wall")
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
        writer.WriteString("Image", value.Image);
        writer.WriteEndObject();
    }
}

public class DeserializerObstacles
{
    public static List<Obstacle> DeserializeObstacles(string json, ObstacleFactory factory)
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new ObstacleConverter(factory) }
        };
        return JsonSerializer.Deserialize<List<Obstacle>>(json, options);
    }
}
