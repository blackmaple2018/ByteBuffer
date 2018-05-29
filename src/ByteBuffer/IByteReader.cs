namespace ByteBuffer
{
    public interface IByteReader
    {
        byte ReadByte();
        bool ReadBool();
        byte[] ReadBytes(int count);
        short ReadShort();
        int ReadInt();
        long ReadLong();
        string ReadString(int count);
    }
}