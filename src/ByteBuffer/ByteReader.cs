using System;
using System.Runtime.InteropServices;
using DotNetCross.Memory;

namespace ByteBuffer
{
    public unsafe class ByteReader : IDisposable
    {
        private readonly GCHandle m_handle;
        protected byte* m_buffer;
        protected readonly int m_start;
        protected readonly int m_length;
        protected int m_index;
        protected bool m_disposed;

        public int Start { get { return m_start; } }	// => m_start;
        public int Length { get { return m_length; } }	// => m_length;

        public int Position { get { return m_index; } }	// => m_index;
        public bool Disposed { get { return m_disposed; } }

        public ByteReader(byte[] buffer, int start, int length)
        {
            m_handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            m_buffer = (byte*)m_handle.AddrOfPinnedObject() + start;
            m_start = start;
            m_length = length;
            m_index = 0;
            m_disposed = false;
        }

        protected byte* Advance(int count)
        {
            var buf = m_buffer;

            if (count <= 0)
                throw new ArgumentOutOfRangeException();

            if (m_index + count > m_length)
                throw new IndexOutOfRangeException();

            m_buffer += count;
            m_index += count;

            return buf;
        }

        public byte ReadByte()
        {
            return *Advance(1);
        }
        public bool ReadBool()
        {
            return *Advance(1) != 0;
        }
        public byte[] ReadBytes(int count)
        {
            var src = Advance(count);
            var data = new byte[count];

            fixed (byte* dst = data)
                Unsafe.CopyBlock(dst, src, (uint)count);

            return data;
        }
        public short ReadShort()
        {
            var src = Advance(2);
            return Unsafe.Read<short>(src);
        }
        public int ReadInt()
        {
            var src = Advance(4);
            return Unsafe.Read<int>(src);
        }
        public long ReadLong()
        {
            var src = Advance(8);
            return Unsafe.Read<long>(src);
        }
        public string ReadString(int count)
        {
            var src = Advance(count);
            return new string((sbyte*)src, 0, count);
        }
        
        public void Skip(int count)
        {
            Advance(count);
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_handle.Free();
                m_disposed = true;
            }
        }
    }
}