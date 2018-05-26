using System;

namespace ByteBuffer
{
    public unsafe class ByteReader
    {
        private readonly byte[] m_buffer;
        private readonly int m_length;
        private readonly int m_start;
        private int m_index;

        public int Length { get { return m_length; } }	// => m_length;
        public int Start { get { return m_start; } }	// => m_start;
        public int Position { get { return m_index; } }	// => m_index;

        public ByteReader(byte[] buffer, int start, int length)
        {
            //TODO: Range checks
            m_buffer = buffer;
            m_length = length;
            m_start = start;
            m_index = start;
        }

        private int Advance(int count)
        {
            var idx = m_index;

            if (m_index + count > m_buffer.Length || count <= 0)
                throw new IndexOutOfRangeException();

            m_index += count;
            return idx;
        }

        public byte ReadByte()
        {
            return m_buffer[Advance(1)];
        }
        public bool ReadBool()
        {
            return ReadByte() != 0;
        }
        public byte[] ReadBytes(int count)
        {
            var idx = Advance(count);
            var data = new byte[count];

            fixed (byte* src = m_buffer)
            fixed (byte* dst = data)
                ByteUtilities.MemCopy(dst, src + idx, count);

            return data;
        }
        public short ReadShort()
        {
            short value;
            var idx = Advance(2);

            fixed (byte* ptr = &m_buffer[idx])
                value = *(short*)ptr;

            return value;
        }
        public int ReadInt()
        {
            int value;
            var idx = Advance(4);

            fixed (byte* ptr = &m_buffer[idx])
                value = *(int*)ptr;

            return value;
        }
        public long ReadLong()
        {
            long value;
            var idx = Advance(8);

            fixed (byte* ptr = &m_buffer[idx])
                value = *(long*)ptr;

            return value;
        }
        public string ReadString(int count)
        {
            var idx = Advance(count);

            fixed (byte* ptr = m_buffer)
            {
                var str = (sbyte*) ptr;
                return new string(str,idx,count);
            }
        }

        public void Skip(int count)
        {
            Advance(count);
        }
    }
}
