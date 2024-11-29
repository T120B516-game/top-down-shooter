namespace Shared
{
    public interface IImageProvider
    {
        object Get(string imageName); // Use 'object' to decouple it from System.Drawing.Bitmap.
    }
}
