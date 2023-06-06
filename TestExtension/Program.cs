namespace TestExtension;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var ext = new TestExtension(new string[] { "-p", "9092" });
        ext.Run();
    }
}