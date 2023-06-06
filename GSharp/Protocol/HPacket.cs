using System.Text;

namespace GSharp.Protocol;

public class HPacket
{
    private bool _isEdited = false;
    private byte[] _bytes = { 0, 0 , 0, 2, 0, 0 };
    private int _readIndex = 6;

    private string? _identifier;
    private HDirection? _direction;

    public HPacket(IEnumerable<byte> bytes)
    {
        _bytes = bytes.ToArray();
        FixLength();
    }

    public HPacket(HPacket packet)
    {
        _bytes = packet.Bytes;
        _isEdited = packet.IsEdited;
    }

    public HPacket(ushort headerId)
    {
        Replace(4, headerId);
        _isEdited = false;
    }

    public HPacket(ushort headerId, byte[] bytes) : this(headerId)
    {
        Append(bytes);
        _isEdited = false;
    }

    public HPacket(string identifier, HDirection direction)
    {
        _identifier = identifier;
        _direction = direction;
    }

    public HPacket(string expression)
    {
        if (expression.StartsWith('1') || expression.StartsWith('0'))
        {
            _isEdited = expression.ToCharArray()[0] == '1';
            _bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(expression.Substring(1));
        }
        else
        {
            // TODO from expression
        }
    }

    public bool IsEdited => _isEdited;

    public byte[] Bytes => _bytes.ToArray();

    public int ReadIndex
    {
        get => _readIndex;
        set => _readIndex = value < 6 ? 6 : value;
    }

    public string? Identifier
    {
        get => _identifier;
        set => _identifier = value;
    }

    public HDirection? Direction
    {
        get => _direction;
        set => _direction = value;
    }

    public int BytesLength => _bytes.Length;

    public int EOF => 
        _readIndex < BytesLength ? 0
        : _readIndex > BytesLength ? 2
        : 1;

    public ushort HeaderId => _bytes.Skip(4).ToUShort();

    public uint Length => _bytes.ToUInt();

    public string Stringified => (_isEdited ? "1" : "0") + Encoding.GetEncoding("ISO-8859-1").GetString(_bytes);

    public bool ReadBool()
    {
        _readIndex += 1;
        return ReadBool(_readIndex - 1);
    }

    public bool ReadBool(int index)
    {
        return _bytes.Skip(index).ToBool();
    }

    public Boolean IsCorrupted()
    {

        if (BytesLength >= 6)
        {
            if (Length == BytesLength - 4)
            {
                return false;
            }
        }

        return true;
    }

    public byte ReadByte()
    {
        _readIndex += 1;
        return ReadByte(_readIndex - 1);
    }

    public byte ReadByte(int index)
    {
        return _bytes.Skip(index).ToByte();
    }

    public short ReadShort()
    {
        _readIndex += 2;
        return ReadShort(_readIndex - 2);
    }

    public short ReadShort(int index)
    {
        return _bytes.Skip(index).ToShort();
    }

    public ushort ReadUShort()
    {
        _readIndex += 2;
        return ReadUShort(_readIndex - 2);
    }

    public ushort ReadUShort(int index)
    {
        return _bytes.Skip(index).ToUShort();
    }

    public int ReadInt()
    {
        _readIndex += 4;
        return ReadInt(_readIndex - 4);
    }

    public int ReadInt(int index)
    {
        return _bytes.Skip(index).ToInt();
    }

    public uint ReadUInt()
    {
        _readIndex += 4;
        return ReadUInt(_readIndex - 4);
    }

    public uint ReadUInt(int index)
    {
        return _bytes.Skip(index).ToUInt();
    }

    public long ReadLong()
    {
        _readIndex += 8;
        return ReadLong(_readIndex - 8);
    }

    public long ReadLong(int index)
    {
        return _bytes.Skip(index).ToLong();
    }

    public ulong ReadULong()
    {
        _readIndex += 8;
        return ReadULong(_readIndex - 8);
    }

    public ulong ReadULong(int index)
    {
        return _bytes.Skip(index).ToULong();
    }

    public float ReadFloat()
    {
        _readIndex += 4;
        return ReadFloat(_readIndex - 4);
    }

    public float ReadFloat(int index)
    {
        return _bytes.Skip(index).ToFloat();
    }

    public double ReadDouble()
    {
        _readIndex += 8;
        return ReadDouble(_readIndex - 8);
    }

    public double ReadDouble(int index)
    {
        return _bytes.Skip(index).ToDouble();
    }

    public byte[] ReadBytes(int length)
    {
        _readIndex += length;
        return ReadBytes(length, _readIndex - length);
    }

    public byte[] ReadBytes(int length, int index)
    {
        return _bytes.Skip(index).Take(length).ToArray();
    }

    public string ReadString()
    {
        var length = ReadUShort();
        _readIndex += length;
        return ReadString(_readIndex - length, length);
    }

    public string ReadString(int index)
    {
        return ReadString(index + 2, ReadUShort(index));
    }

    private string ReadString(int index, int length)
    {
        return Encoding.UTF8.GetString(_bytes.Skip(index).Take(length).ToArray());
    }

    public string ReadLongString()
    {
        var length = ReadInt();
        _readIndex += length;
        return ReadString(_readIndex - length, length);
    }

    public string ReadLongString(int index)
    {
        return ReadString(index + 4, ReadInt(index));
    }
    
    // TODO optional read structure

    public void Replace(int index, byte[] bytes)
    {
        bytes.CopyTo(_bytes, index);
        FixLength();
        _isEdited = true;
    }
    
    public void Replace(int index, bool b)
    {
        Replace(index, BitConverter.GetBytes(b));
    }
    
    public void Replace(int index, byte b)
    {
        Replace(index, new []{ b });
    }
    
    public void Replace(int index, short s)
    {
        Replace(index, BitConverter.GetBytes(s).Reverse().ToArray());
    }
    
    public void Replace(int index, ushort us)
    {
        Replace(index, BitConverter.GetBytes(us).Reverse().ToArray());
    }
    
    public void Replace(int index, int i)
    {
        Replace(index, BitConverter.GetBytes(i).Reverse().ToArray());
    }
    
    public void Replace(int index, uint ui)
    {
        Replace(index, BitConverter.GetBytes(ui).Reverse().ToArray());
    }
    
    public void Replace(int index, long l)
    {
        Replace(index, BitConverter.GetBytes(l).Reverse().ToArray());
    }
    
    public void Replace(int index, ulong ul)
    {
        Replace(index, BitConverter.GetBytes(ul).Reverse().ToArray());
    }
    
    public void Replace(int index, float f)
    {
        Replace(index, BitConverter.GetBytes(f).Reverse().ToArray());
    }
    
    public void Replace(int index, double d)
    {
        Replace(index, d.ToBytes());
    }

    public void Replace(int index, string str)
    {
        var oldLength = ReadUShort(index);
        _bytes = _bytes.Take(index)
            .Concat(str.ToBytes())
            .Concat(_bytes.Skip(index + 2 + oldLength))
            .ToArray();
        FixLength();
        _isEdited = true;
    }

    public void Insert(int index, byte[] bytes)
    {
        _bytes = _bytes
            .Take(index)
            .Concat(bytes)
            .Concat(_bytes.Skip(index))
            .ToArray();
        FixLength();
        _isEdited = true;
    }

    public void Insert(int index, bool b)
    {
        Insert(index, b.ToBytes());
    }

    public void Insert(int index, byte b)
    {
        Insert(index, b.ToBytes());
    }

    public void Insert(int index, short s)
    {
        Insert(index, s.ToBytes());
    }

    public void Insert(int index, ushort us)
    {
        Insert(index, us.ToBytes());
    }

    public void Insert(int index, int i)
    {
        Insert(index, i.ToBytes());
    }

    public void Insert(int index, uint ui)
    {
        Insert(index, ui.ToBytes());
    }

    public void Insert(int index, long l)
    {
        Insert(index, l.ToBytes());
    }

    public void Insert(int index, ulong ul)
    {
        Insert(index, ul.ToBytes());
    }

    public void Insert(int index, float f)
    {
        Insert(index, f.ToBytes());
    }

    public void Insert(int index, double d)
    {
        Insert(index, d.ToBytes());
    }

    public void Insert(int index, string s)
    {
        Insert(index, s.ToBytes());
    }

    public void Append(byte[] bytes)
    {
        Insert(_bytes.Length, bytes);
    }

    public void Append(bool b)
    {
        Insert(_bytes.Length, b);
    }

    public void Append(byte b)
    {
        Insert(_bytes.Length, b);
    }

    public void Append(short s)
    {
        Insert(_bytes.Length, s);
    }

    public void Append(ushort us)
    {
        Insert(_bytes.Length, us);
    }

    public void Append(int i)
    {
        Insert(_bytes.Length, i);
    }

    public void Append(uint ui)
    {
        Insert(_bytes.Length, ui);
    }

    public void Append(long l)
    {
        Insert(_bytes.Length, l);
    }

    public void Append(ulong ul)
    {
        Insert(_bytes.Length, ul);
    }

    public void Append(float f)
    {
        Insert(_bytes.Length, f);
    }

    public void Append(double d)
    {
        Insert(_bytes.Length, d);
    }

    public void Append(string s)
    {
        Insert(_bytes.Length, s);
    }

    internal void AppendLongString(string s)
    {
        var bytes = Encoding.UTF8.GetBytes(s);
        Append(bytes.Length);
        Append(bytes);
    }

    public void FixLength()
    {
        var length = BytesLength - 4;
        length.ToBytes().CopyTo(_bytes, 0);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == this)
            return true;
        if (obj == null)
            return false;
        if (obj.GetType() != typeof(HPacket))
            return false;

        return Equals((HPacket)obj);
    }

    protected bool Equals(HPacket other)
    {
        return _isEdited == other._isEdited && _bytes.Equals(other._bytes) && _identifier == other._identifier && _direction == other._direction;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_isEdited, _bytes, _identifier, _direction);
    }

    internal void constructFromString(string v)
    {
        throw new NotImplementedException();
    }
}
