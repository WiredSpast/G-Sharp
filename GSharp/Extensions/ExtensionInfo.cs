namespace GSharp.Extensions
{
    public class ExtensionInfo : Attribute
    {
        public string Title { get; }
        public string Description { get; }
        public string Version { get; }
        public string Author { get; }

        public ExtensionInfo(string title, string description, string version, string author)
        {
            Title = title;
            Description = description;
            Version = version;
            Author = author;
        }
    }
}
