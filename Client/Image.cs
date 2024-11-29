namespace Client
{
    public class Image
    {
        public string FilePath { get; } // „Intrinsic“ būsena: bendrinama
        private readonly System.Drawing.Image _image;

        public Image(string filePath)
        {
            FilePath = filePath;
            _image = System.Drawing.Image.FromFile(filePath);
        }
    }
}
