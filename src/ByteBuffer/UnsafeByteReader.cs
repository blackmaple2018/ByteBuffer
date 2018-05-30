using DotNetCross.Memory;

namespace ByteBuffer
{
        	: this(buffer, 0, buffer.Length)
	public unsafe class UnsafeByteReader : ByteReader
	{
		public UnsafeByteReader(byte[] buffer, int start, int length)
			: base(buffer, start, length)
		{
		}
		public UnsafeByteReader(byte[] buffer)
			: this(buffer, 0, buffer.Length)
		{
		}

		public T Read<T>()
		{
			var size = Unsafe.SizeOf<T>();
			var src = Advance(size);
			return Unsafe.Read<T>(src);
		}
	}
}