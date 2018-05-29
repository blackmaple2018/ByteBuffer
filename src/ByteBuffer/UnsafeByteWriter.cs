using DotNetCross.Memory;

namespace ByteBuffer
{
    public unsafe class UnsafeByteWriter : ByteWriter
    {
        public UnsafeByteWriter(uint bufferSize) 
            : base(bufferSize)
        {
        }
        public UnsafeByteWriter()
        	: base(DefaultBufferSize)
        {        
        }

        public void Write<T>(T value)
        {
            var size = Unsafe.SizeOf<T>();
            var idx = Advance(size);

            fixed (byte* ptr = &m_buffer[idx])
                Unsafe.Write(ptr, value);
        }
    }
}