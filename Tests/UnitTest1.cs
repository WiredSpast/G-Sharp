namespace Tests;

public class HPacketTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ConstructFromIdentifierAndDirection()
    {
        var packet = new HPacket("Chat", HDirection.ToClient);
        Assert.Multiple(() =>
        {
            Assert.That(packet.Identifier, Is.EqualTo("Chat"));
            Assert.That(packet.Direction, Is.EqualTo(HDirection.ToClient));
            Assert.That(packet.IsEdited, Is.False);
            Assert.That(packet.Bytes, Is.EqualTo(new byte[] { 0, 0, 0, 2, 0, 0 }));
            Assert.That(packet.ReadIndex, Is.EqualTo(6));
        });
    }

    [Test]
    public void ConstructFromBytes()
    {
        var packet = new HPacket(new byte[] { 0, 0, 0, 15, 0, 15, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 3 });
        Assert.Multiple(() =>
        {
            Assert.That(packet.Identifier, Is.Null);
            Assert.That(packet.Direction, Is.Null);
            Assert.That(packet.IsEdited, Is.False);
            Assert.That(packet.Bytes, Is.EqualTo(new byte[] { 0, 0, 0, 15, 0, 15, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 3 }));
            Assert.That(packet.ReadIndex, Is.EqualTo(6));
        });
    }

    [Test]
    public void ConstructFromStringified()
    {
        var packet = new HPacket(new byte[] { 0, 0, 0, 15, 0, 15, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 3 });
        var stringified = packet.Stringified;
        var packet2 = new HPacket(stringified);
        Assert.Multiple(() =>
        {
            Assert.That(packet.Identifier, Is.Null);
            Assert.That(packet.Direction, Is.Null);
            Assert.That(packet.IsEdited, Is.False);
            Assert.That(packet.Bytes, Is.EqualTo(new byte[] { 0, 0, 0, 15, 0, 15, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 3 }));
            Assert.That(packet.ReadIndex, Is.EqualTo(6));
        });
    }

    [Test]
    public void ReadInt()
    {
        var packet = new HPacket(new byte[] { 0, 0, 0, 15, 0, 15, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 3 });
        Assert.Multiple(() =>
        {
            Assert.That(packet.ReadIndex, Is.EqualTo(6));
            Assert.That(packet.ReadInt(), Is.EqualTo(1));
            Assert.That(packet.ReadIndex, Is.EqualTo(10));
            Assert.That(packet.ReadInt(), Is.EqualTo(2));
            Assert.That(packet.ReadIndex, Is.EqualTo(14));
            Assert.That(packet.ReadInt(), Is.EqualTo(3));
        });
    }

    [Test]
    public void ReplaceBytes()
    {
        var packet = new HPacket(new byte[] { 0, 0, 0, 15, 0, 15, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 3 });
        Console.WriteLine(string.Join(", ", packet.Bytes));
        packet.Replace(6, new byte[] { 0, 0, 0, 4 });
        Console.WriteLine(string.Join(", ", packet.Bytes));
        Assert.Multiple(() =>
        {
            Assert.That(packet.ReadInt(), Is.EqualTo(4));
            Assert.That(packet.ReadInt(), Is.EqualTo(2));
            Assert.That(packet.ReadInt(), Is.EqualTo(3));
        });
    }

    [Test]
    public void ReplaceInt()
    {
        var packet = new HPacket(new byte[] { 0, 0, 0, 15, 0, 15, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 3 });
        Console.WriteLine(string.Join(", ", packet.Bytes));
        packet.Replace(6, 4);
        Console.WriteLine(string.Join(", ", packet.Bytes));
        Assert.Multiple(() =>
        {
            Assert.That(packet.ReadInt(), Is.EqualTo(4));
            Assert.That(packet.ReadInt(), Is.EqualTo(2));
            Assert.That(packet.ReadInt(), Is.EqualTo(3));
        });
    }

    [Test]
    public void InsertInt()
    {
        var packet = new HPacket(new byte[] { 0, 0, 0, 15, 0, 15, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 3 });
        Console.WriteLine(string.Join(", ", packet.Bytes));
        packet.Insert(10, -1);
        Console.WriteLine(string.Join(", ", packet.Bytes));
        Assert.Multiple(() =>
        {
            Assert.That(packet.ReadInt(), Is.EqualTo(1));
            Assert.That(packet.ReadInt(), Is.EqualTo(-1));
            Assert.That(packet.ReadInt(), Is.EqualTo(2));
            Assert.That(packet.ReadInt(), Is.EqualTo(3));
        });
    }

    [Test]
    public void AppendInt()
    {
        var packet = new HPacket(new byte[] { 0, 0, 0, 15, 0, 15, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 3 });
        Console.WriteLine(string.Join(", ", packet.Bytes));
        packet.Append(4);
        Console.WriteLine(string.Join(", ", packet.Bytes));
        Assert.Multiple(() =>
        {
            Assert.That(packet.ReadInt(), Is.EqualTo(1));
            Assert.That(packet.ReadInt(), Is.EqualTo(2));
            Assert.That(packet.ReadInt(), Is.EqualTo(3));
            Assert.That(packet.ReadInt(), Is.EqualTo(4));
        });
    }
}