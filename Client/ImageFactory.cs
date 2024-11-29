using Shared;

namespace Client
{
    public class ImageFactory : IImageProvider
    {
        private readonly Dictionary<string, Bitmap> _imageCache = new();
        private readonly string _imageDirectory;

        public ImageFactory()
        {
            _imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
        }

        public object Get(string imageName)
        {
            if (!_imageCache.ContainsKey(imageName))
            {
                string imagePath = Path.Combine(_imageDirectory, imageName);

                if (File.Exists(imagePath))
                {
                    _imageCache[imageName] = new Bitmap(imagePath);
                }
                else
                {
                    throw new FileNotFoundException($"Image '{imageName}' not found at '{imagePath}'");
                }
            }

            return _imageCache[imageName];
        }
    }
}
