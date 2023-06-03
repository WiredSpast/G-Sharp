using System.Text;

namespace GSharp.Protocol;

public static class ByteRep
{
    // Ignore how dumb these first two methods are
    public static byte ToByte(this IEnumerable<byte> bytes)
    {
        return bytes.First();
    }
    
    public static byte[] ToBytes(this byte b)
    {
        return new [] { b };
    }
    
    public static bool ToBool(this IEnumerable<byte> bytes)
    {
        return BitConverter.ToBoolean(bytes.Take(1).ToArray());
    }
    
    public static byte[] ToBytes(this bool b)
    {
        return BitConverter.GetBytes(b);
    }
    
    public static short ToShort(this IEnumerable<byte> bytes)
    {
        return BitConverter.ToInt16(ReverseIfLittleEndian(bytes.Take(2).ToArray()));
    }
    
    public static byte[] ToBytes(this short s)
    {
        return ReverseIfLittleEndian(BitConverter.GetBytes(s));
    }
    
    public static ushort ToUShort(this IEnumerable<byte> bytes)
    {
        return BitConverter.ToUInt16(ReverseIfLittleEndian(bytes.Take(2).ToArray()));
    }
    
    public static byte[] ToBytes(this ushort us)
    {
        return ReverseIfLittleEndian(BitConverter.GetBytes(us));
    }
    
    public static int ToInt(this IEnumerable<byte> bytes)
    {
        return BitConverter.ToInt32(ReverseIfLittleEndian(bytes.Take(4).ToArray()));
    }
    
    public static byte[] ToBytes(this int i)
    {
        return ReverseIfLittleEndian(BitConverter.GetBytes(i));
    }
    
    public static uint ToUInt(this IEnumerable<byte> bytes)
    {
        return BitConverter.ToUInt32(ReverseIfLittleEndian(bytes.Take(4).ToArray()));
    }
    
    public static byte[] ToBytes(this uint ui)
    {
        return ReverseIfLittleEndian(BitConverter.GetBytes(ui));
    }
    
    public static long ToLong(this IEnumerable<byte> bytes)
    {
        return BitConverter.ToInt64(ReverseIfLittleEndian(bytes.Take(8).ToArray()));
    }
    
    public static byte[] ToBytes(this long l)
    {
        return ReverseIfLittleEndian(BitConverter.GetBytes(l));
    }
    
    public static ulong ToULong(this IEnumerable<byte> bytes)
    {
        return BitConverter.ToUInt64(ReverseIfLittleEndian(bytes.Take(8).ToArray()));
    }
    
    public static byte[] ToBytes(this ulong ul)
    {
        return ReverseIfLittleEndian(BitConverter.GetBytes(ul));
    }
    
    public static float ToFloat(this IEnumerable<byte> bytes)
    {
        return BitConverter.ToSingle(ReverseIfLittleEndian(bytes.Take(4).ToArray()));
    }
    
    public static byte[] ToBytes(this float f)
    {
        return ReverseIfLittleEndian(BitConverter.GetBytes(f));
    }
    
    public static double ToDouble(this IEnumerable<byte> bytes)
    {
        return BitConverter.ToDouble(ReverseIfLittleEndian(bytes.Take(8).ToArray()));
    }
    
    public static byte[] ToBytes(this double d)
    {
        return ReverseIfLittleEndian(BitConverter.GetBytes(d));
    }
    
    public static byte[] ToBytes(this string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        var length = (ushort)bytes.Length;
        return length.ToBytes().Concat(bytes).ToArray();
    }

    private static byte[] ReverseIfLittleEndian(byte[] bytes)
    {
        return BitConverter.IsLittleEndian
            ? bytes.Reverse().ToArray()
            : bytes;
    }
}