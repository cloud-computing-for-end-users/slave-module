namespace screen_capture_api.Model
{
    public class PathToScreen
    {
        public string Path { get; }

        public PathToScreen(string path)
        {
            Path = path;
        }

        public override string ToString()
        {
            return Path;
        }
    }
}