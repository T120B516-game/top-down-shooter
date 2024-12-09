using Shared;
using System.Drawing;

namespace Tests.Obstacle
{
    public class ObstacleTests
    {
        /*[Fact]
        public void PenetratableObstacle_Draw_ShouldFillRectangleWithGreenBrush()
        {
            // Arrange
            var penetratable = new Penetratable
            {
                X = 10,
                Y = 10,
                Width = 20,
                Height = 20
            };

            using var bitmap = new Bitmap(40, 40);
            using var graphics = Graphics.FromImage(bitmap);

            // Act
            penetratable.Draw(graphics);

            // Assert
            Color pixelColor = bitmap.GetPixel(15, 15); // A point inside the rectangle
            Color expectedColor = Color.FromArgb(255, 0, 128, 0); // Color.Green ARGB values
            Assert.Equal(expectedColor, pixelColor);
        }

        [Fact]
        public void UnpenetratableObstacle_Draw_ShouldFillRectangleWithRedBrush()
        {
            // Arrange
            var unpenetratable = new Unpenetratable
            {
                X = 10,
                Y = 10,
                Width = 20,
                Height = 20
            };

            using var bitmap = new Bitmap(40, 40);
            using var graphics = Graphics.FromImage(bitmap);

            // Act
            unpenetratable.Draw(graphics);

            // Assert
            Color pixelColor = bitmap.GetPixel(15, 15); // A point inside the rectangle
            Color expectedColor = Color.FromArgb(255, 255, 0, 0); // Color.Red ARGB values
            Assert.Equal(expectedColor, pixelColor);
        }*/
    }

    public class PenetratableObstacleBuilderTests
    {
        [Fact]
        public void Build_ShouldCreatePenetratableObstacleWithCorrectProperties()
        {
            // Arrange
            var builder = new PenetratableObstacleBuilder();

            // Act
            var obstacle = builder.SetPosition(10, 20)
                .SetSize(30, 40)
                .Build();

            // Assert
            Assert.IsType<Penetratable>(obstacle);
            Assert.Equal(10, obstacle.X);
            Assert.Equal(20, obstacle.Y);
            Assert.Equal(30, obstacle.Width);
            Assert.Equal(40, obstacle.Height);
            Assert.Equal("Penetratable", obstacle.Type);
        }
    }

    public class UnpenetratableObstacleBuilderTests
    {
        [Fact]
        public void Build_ShouldCreateUnpenetratableObstacleWithCorrectProperties()
        {
            // Arrange
            var builder = new UnpenetratableObstacleBuilder();

            // Act
            var obstacle = builder.SetPosition(50, 60)
                .SetSize(70, 80)
                .Build();

            // Assert
            Assert.IsType<Unpenetratable>(obstacle);
            Assert.Equal(50, obstacle.X);
            Assert.Equal(60, obstacle.Y);
            Assert.Equal(70, obstacle.Width);
            Assert.Equal(80, obstacle.Height);
            Assert.Equal("Unpenetratable", obstacle.Type);
        }
    }

    public class ObstacleFactoryTests
    {
        [Fact]
        public void CreatePenetratable_ShouldReturnPenetratableObstacleWithGivenProperties()
        {
            // Arrange
            var factory = new ObstacleFactory();

            // Act
            var obstacle = factory.CreatePenetratable(100, 200, 30, 40);

            // Assert
            Assert.IsType<Penetratable>(obstacle);
            Assert.Equal(100, obstacle.X);
            Assert.Equal(200, obstacle.Y);
            Assert.Equal(30, obstacle.Width);
            Assert.Equal(40, obstacle.Height);
            Assert.Equal("Penetratable", obstacle.Type);
        }

        [Fact]
        public void CreateUnpenetratable_ShouldReturnUnpenetratableObstacleWithGivenProperties()
        {
            // Arrange
            var factory = new ObstacleFactory();

            // Act
            var obstacle = factory.CreateUnpenetratable(150, 250, 35, 45);

            // Assert
            Assert.IsType<Unpenetratable>(obstacle);
            Assert.Equal(150, obstacle.X);
            Assert.Equal(250, obstacle.Y);
            Assert.Equal(35, obstacle.Width);
            Assert.Equal(45, obstacle.Height);
            Assert.Equal("Unpenetratable", obstacle.Type);
        }
    }
}
