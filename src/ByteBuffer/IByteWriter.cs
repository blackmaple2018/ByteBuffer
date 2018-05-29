namespace ByteBuffer
{
	public interface IByteWriter
	{
		void WriteByte(byte value);
		void WriteBool(bool value);
		void WriteBytes(byte[] buffer, int start, int length);
		void WriteShort(short value);
		void WriteInt(int value);
		void WriteLong(long value);
		void WriteString(string value);
	}
}