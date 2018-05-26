using System;
using DotNetCross.Memory;

namespace ByteBuffer
{
    public unsafe class ByteWriter
    {
        public const uint DefaultBufferSize = 1024;

        protected byte[] m_buffer;
        protected uint m_size;

        /// <summary>
        /// Clear the old buffer when we resize internally
        /// so the contents doesn't stick around in memory
        /// </summary>
        public bool ClearBuffer { get; set; }
        public uint Length { get { return m_size; } }   // => m_size;

        public ByteWriter(uint bufferSize = DefaultBufferSize)
        {
            m_buffer = new byte[bufferSize];
            m_size = 0;
            ClearBuffer = false;
        }

        protected uint Advance(uint count)
        {
            var idx = m_size;
            var bufferSize = m_buffer.Length;
            var required = m_size + count;

            while (bufferSize < required)
                bufferSize *= 2;

            if (m_buffer.Length != bufferSize)
            {
                var newBuffer = new byte[bufferSize];

                fixed (byte* src = m_buffer)
                fixed (byte* dst = newBuffer)
                {
                    Unsafe.CopyBlock(dst, src, m_size);

                    if (ClearBuffer)
                        Unsafe.InitBlock(src, 0, m_size);
                }

                m_buffer = newBuffer;
            }

            m_size += count;

            return idx;
        }

        public void WriteByte(byte value)
        {
            var idx = Advance(1);
            m_buffer[idx] = value;
        }
        public void WriteBool(bool value)
        {
            var idx = Advance(1);
            m_buffer[idx] = value ? (byte)1 : (byte)0;
        }
        public void WriteBytes(byte[] buffer, int start, uint length)
        {
            //TODO: Range checks
            var idx = Advance(length);

            fixed (byte* src = buffer)
            fixed (byte* dst = m_buffer)
                Unsafe.CopyBlock(dst + idx, src + start, length);
        }
        public void WriteShort(short value)
        {
            var idx = Advance(2);

            fixed (byte* ptr = &m_buffer[idx])
                *(short*)ptr = value;
        }
        public void WriteInt(int value)
        {
            var idx = Advance(4);

            fixed (byte* ptr = &m_buffer[idx])
                *(int*)ptr = value;
        }
        public void WriteLong(long value)
        {
            var idx = Advance(8);

            fixed (byte* ptr = &m_buffer[idx])
                *(long*)ptr = value;
        }
        public void WriteString(string value)
        {
            var length = (uint)value.Length;
            var idx = Advance(length);

            fixed (char* strPtr = value)
            fixed (byte* bufPtr = m_buffer)
            {
                var str = strPtr;
                var buf = bufPtr + idx;

                for (int i = 0; i < length; i++)
                    *buf++ = (byte)*str++;
            }
        }

        public ArraySegment<byte> GetArraySegment()
        {
            return new ArraySegment<byte>(m_buffer, 0, (int)m_size);
        }
        public byte[] GetBuffer()
        {
            var newBuffer = new byte[m_size];

            fixed (byte* src = m_buffer)
            fixed (byte* dst = newBuffer)
                Unsafe.CopyBlock(dst, src, m_size);

            return newBuffer;
        }
    }
}