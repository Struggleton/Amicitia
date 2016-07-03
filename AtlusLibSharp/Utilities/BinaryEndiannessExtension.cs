namespace AtlusLibSharp.Utilities
{
    using OpenTK;
    using System;
    using System.IO;
    using System.Text;
    using System.Drawing;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class EndiannessReader : BinaryReader
    {
        // Constructors
        public EndiannessReader(Stream stream) : base(stream) { }
        public EndiannessReader(Stream stream, Endianness endianness = Endianness.Little) : base(stream)
        {
            _Endianness = endianness;
        }

        public EndiannessReader(Stream stream, Encoding encoding, Endianness endianness = Endianness.Little) : base(stream)
        {
            _Endianness = endianness;
            _Encoding = encoding;
        }

        // Extension Methods
        public Color ReadColor()
        {
            Bytes = ReadBytes(4);
            return Color.FromArgb(BitConverter.ToInt32(Bytes, 0));
        }

        public Vector2 ReadVector2()
        {
            return new Vector2(ReadSingle(), ReadSingle());
        }
        public Vector3 ReadVector3()
        {
            return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
        }
        public Matrix4 ReadMatrix4()
        {
            Matrix4 mtx = new Matrix4();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    mtx[i, j] = ReadSingle();
                }
            }
            return mtx;
        }
        public Matrix3x4 ReadMatrix3x4()
        {
            Matrix3x4 mtx = new Matrix3x4();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    mtx[i, j] = ReadSingle();
                }
            }
            return mtx;
        }
        public Matrix4x3 ReadMatrix4x3()
        {
            Matrix4x3 mtx = new Matrix4x3();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mtx[i, j] = ReadSingle();
                }
            }
            return mtx;
        }

        public string ReadCString()
        {
            List<byte> bytes = new List<byte>();
            byte b = ReadByte();
            while (b != 0)
            {
                bytes.Add(b);
                b = ReadByte();
            }
            return _Encoding.GetString(bytes.ToArray());
        }

        public string ReadCString(int length)
        {
            return _Encoding.GetString(ReadBytes(length)).Trim('\0');
        }

        public string ReadCStringAligned()
        {
            string str = ReadCString();
            AlignPosition(4);
            return str;
        }

        public string ReadCStringAtOffset(int offset)
        {
            long posStart = GetPosition();
            Seek(offset, SeekOrigin.Begin);
            string str = ReadCString();
            Seek(posStart, SeekOrigin.Begin);
            return str;
        }

        public byte[] ReadBytesAtOffset(int count, long offset)
        {
            long returnPosition = GetPosition();
            Seek(offset, SeekOrigin.Begin);
            byte[] data = ReadBytes(count);
            Seek(returnPosition, SeekOrigin.Begin);
            return data;
        }

        // Array Reading
        public short[] ReadInt16Array(int count)
        {
            short[] arr = new short[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadInt16();
            }
            return arr;
        }

        public ushort[] ReadUInt16Array(int count)
        {
            ushort[] arr = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadUInt16();
            }
            return arr;
        }

        public int[] ReadInt32Array(int count)
        {
            int[] arr = new int[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadInt32();
            }
            return arr;
        }

        public uint[] ReadUInt32Array(int count)
        {
            uint[] arr = new uint[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadUInt32();
            }
            return arr;
        }

        public float[] ReadFloatArray(int count)
        {
            float[] arr = new float[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadSingle();
            }
            return arr;
        }

        public double[] ReadDoubleArray(int count)
        {
            double[] arr = new double[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadDouble();
            }
            return arr;
        }

        public Color[] ReadColorArray(int count)
        {
            Color[] arr = new Color[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadColor();
            }
            return arr;
        }

        public string[] ReadCStringArray(int count, int length = -1)
        {
            string[] arr = new string[count];
            for (int i = 0; i < count; i++)
            {
                if (length != -1)
                {
                    arr[i] = ReadCString(length);
                }
                else
                {
                    arr[i] = ReadCString();
                }
            }
            return arr;
        }

        public Vector2[] ReadVector2Array(int count)
        {
            Vector2[] arr = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadVector2();
            }
            return arr;
        }

        public Vector3[] ReadVector3Array(int count)
        {
            Vector3[] arr = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadVector3();
            }
            return arr;
        }

        public Matrix4[] ReadMatrix4Array(int count)
        {
            Matrix4[] arr = new Matrix4[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadMatrix4();
            }
            return arr;
        }

        public Matrix3x4[] ReadMatrix3x4Array(int count)
        {
            Matrix3x4[] arr = new Matrix3x4[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadMatrix3x4();
            }
            return arr;
        }

        public Matrix4x3[] ReadMatrix4x3Array(int count)
        {
            Matrix4x3[] arr = new Matrix4x3[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = ReadMatrix4x3();
            }
            return arr;
        }

        public T ReadStructure<T>()
        {
            int structureSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[structureSize];

            if (Read(buffer, 0, structureSize) != structureSize)
            {
                throw new EndOfStreamException("could not read all of data for structure");
            }
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }

        public T ReadStructure<T>(int size)
        {
            int structureSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[Math.Max(structureSize, size)];
            if (Read(buffer, 0, size) != size)
            {
                throw new EndOfStreamException("could not read all of data for structure");
            }
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }

        public T[] ReadStructures<T>(int count)
        {
            int structureSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[structureSize * count];
            if (Read(buffer, 0, structureSize * count) != structureSize * count)
            {
                throw new EndOfStreamException("could not read all of data for structures");
            }
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T[] structArray = new T[count];
            IntPtr bufferPtr = handle.AddrOfPinnedObject();
            for (int i = 0; i < count; i++)
            {
                structArray[i] = (T)Marshal.PtrToStructure(bufferPtr, typeof(T));
                bufferPtr += structureSize;
            }
            handle.Free();
            return structArray;
        }

        public long GetPosition()
        {
            return BaseStream.Position;
        }

        public long GetLength()
        {
            return BaseStream.Length;
        }

        public void SetPosition(long position)
        {
            BaseStream.Position = position;
        }

        public void AlignPosition(int alignmentBytes)
        {
            BaseStream.Position = AlignmentHelper.Align(BaseStream.Position, alignmentBytes);
        }

        public void Seek(long position, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    BaseStream.Position = position;
                    break;
                case SeekOrigin.Current:
                    BaseStream.Position += position;
                    break;
                case SeekOrigin.End:
                    BaseStream.Position = BaseStream.Length - position;
                    break;
            }
        }

        public void SetEndianness(Endianness endianness)
        {
            _Endianness = endianness;
        }

        // Public Overrides
        public override short ReadInt16()
        {
            Bytes = ReadBytes(2);
            return BitConverter.ToInt16(Bytes, 0);
        }
        public override ushort ReadUInt16()
        {
            Bytes = ReadBytes(2);
            return BitConverter.ToUInt16(Bytes, 0);
        }
        public override int ReadInt32()
        {
            Bytes = ReadBytes(4);
            return BitConverter.ToInt32(Bytes, 0);
        }
        public override uint ReadUInt32()
        {
            Bytes = ReadBytes(4);
            return BitConverter.ToUInt32(Bytes, 0);
        }
        public override long ReadInt64()
        {
            Bytes = ReadBytes(8);
            return BitConverter.ToInt64(Bytes, 0);
        }
        public override ulong ReadUInt64()
        {
            Bytes = ReadBytes(4);
            return BitConverter.ToUInt64(Bytes, 0);
        }
        public override float ReadSingle()
        {
            Bytes = ReadBytes(4);
            return BitConverter.ToSingle(Bytes, 0);
        }
        public override double ReadDouble()
        {
            Bytes = ReadBytes(8);
            return BitConverter.ToSingle(Bytes, 0);
        }

        // Private Fields
        private Encoding _Encoding = Encoding.GetEncoding("Shift_JIS");
        private Endianness _Endianness;
        private byte[] _Bytes;

        // Accessors
        private byte[] Bytes
        {
            get { return _Bytes; }
            set
            {
                _Bytes = value;
                if (_Endianness == Endianness.Big)
                {
                    Array.Reverse(_Bytes);
                }
            }
        }
    }

    public class EndiannessWriter : BinaryWriter
    {
        // Constructors
        public EndiannessWriter(Stream stream) : base(stream) { }
        public EndiannessWriter(Stream stream, Endianness endianness = Endianness.Little) : base(stream)
        {
            _Endianness = endianness;
        }
        public EndiannessWriter(Stream stream, Encoding encoding, Endianness endianness = Endianness.Little) : base(stream)
        {
            _Endianness = endianness;
            _Encoding = encoding;
        }

        // Overrides
        public override void Write(short value)
        {
            Bytes = BitConverter.GetBytes(value);
            base.Write(Bytes);
        }

        public override void Write(int value)
        {
            Bytes = BitConverter.GetBytes(value);
            base.Write(Bytes);
        }

        public override void Write(long value)
        {
            Bytes = BitConverter.GetBytes(value);
            base.Write(Bytes);
        }

        public override void Write(ushort value)
        {
            Bytes = BitConverter.GetBytes(value);
            base.Write(Bytes);
        }

        public override void Write(uint value)
        {
            Bytes = BitConverter.GetBytes(value);
            base.Write(Bytes);
        }

        public override void Write(ulong value)
        {
            Bytes = BitConverter.GetBytes(value);
            base.Write(Bytes);
        }

        public override void Write(float value)
        {
            Bytes = BitConverter.GetBytes(value);
            base.Write(Bytes);
        }

        public override void Write(double value)
        {
            Bytes = BitConverter.GetBytes(value);
            base.Write(Bytes);
        }

        // Extensions
        public void Write(Color value)
        {
            Write(value.ToArgb());
        }

        public void Write(Color[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Write(array[i]);
            }
        }

        public void Write(Vector2 value)
        {
            Write(value.X);
            Write(value.Y);
        }

        public void Write(Vector2[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Write(array[i]);
            }
        }

        public void Write(Vector3 value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
        }

        public void Write(Vector3[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Write(array[i]);
            }
        }

        public void Write(Matrix4 value)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Write(value[i, j]);
                }
            }
        }

        public void Write(Matrix4[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Write(array[i]);
            }
        }

        public  void Write(Matrix3x4 value)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Write(value[i, j]);
                }
            }
        }

        public void Write(Matrix3x4[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Write(array[i]);
            }
        }

        public void Write(Matrix4x3 value)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Write(value[i, j]);
                }
            }
        }

        public void Write(Matrix4x3[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Write(array[i]);
            }
        }

        public void WriteCString(string value)
        {
            Write(_Encoding.GetBytes(value));
            Write((byte)0);
        }

        public void WriteCString(string value, int length)
        {
            if (value.Length > length)
            {
                throw new ArgumentException();
            }

            Write(_Encoding.GetBytes(value));
            Write(new byte[length - _Encoding.GetByteCount(value)]);
        }

        public void WriteCStringAligned(string value)
        {
            Write(_Encoding.GetBytes(value));
            AlignPosition(4);
        }

        public void Write(byte value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Write(value);
            }

        }
        public void Write(byte[] value, long offset)
        {
            long posStart = GetPosition();
            Seek(offset, SeekOrigin.Begin);
            Write(value);
            Seek(posStart, SeekOrigin.Begin);
        }

        public void Write(short[] value)
        {
            foreach (short item in value)
            {
                Write(item);
            }
        }

        public void Write(ushort[] value)
        {
            foreach (ushort item in value)
            {
                Write(item);
            }
        }

        public void Write(int[] value)
        {
            foreach (int item in value)
            {
                Write(item);
            }
        }

        public void Write(uint[] value)
        {
            foreach (uint item in value)
            {
                Write(item);
            }
        }

        public void Write(long[] value)
        {
            foreach (long item in value)
            {
                Write(item);
            }
        }

        public void Write(ulong[] value)
        {
            foreach (ulong item in value)
            {
                Write(item);
            }
        }

        public void Write(float[] value)
        {
            foreach (float item in value)
            {
                Write(item);
            }
        }
        public void Write(double[] value)
        {
            foreach (double item in value)
            {
                Write(item);
            }
        }

        // Structure Writing
        public void WriteStructure<T>(T structure)
        {
            int structureSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[structureSize];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(structure, handle.AddrOfPinnedObject(), false);
            handle.Free();
            Bytes = buffer;
            Write(Bytes, 0, buffer.Length);
        }

        public void WriteStructure<T>(T structure, int size)
        {
            int structureSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[Math.Max(structureSize, size)];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(structure, handle.AddrOfPinnedObject(), false);
            handle.Free();
            Bytes = buffer;
            Write(Bytes, 0, buffer.Length);
        }

        public void WriteStructures<T>(T[] structArray, int count)
        {
            int structureSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[structureSize * count];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr bufferPtr = handle.AddrOfPinnedObject();

            for (int i = 0; i < count; i++)
            {
                Marshal.StructureToPtr(structArray[i], bufferPtr, false);
                bufferPtr += structureSize;
            }
            handle.Free();
            Bytes = buffer;
            Write(Bytes, 0, buffer.Length);
        }

        // Assistant Functions
        public void Seek(long position, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    BaseStream.Position = position;
                    break;
                case SeekOrigin.Current:
                    BaseStream.Position += position;
                    break;
                case SeekOrigin.End:
                    BaseStream.Position = BaseStream.Length - position;
                    break;
            }
        }

        public long GetPosition()
        {
            return BaseStream.Position;
        }

        public long GetLength()
        {
            return BaseStream.Length;
        }

        public void SetPosition(long position)
        {
            BaseStream.Position = position;
        }

        public void AlignPosition(int alignmentBytes)
        {
            long align = AlignmentHelper.Align(GetPosition(), alignmentBytes);
            Write(new byte[(align - GetPosition())]);
        }

        public void SetEndianness(Endianness endianness)
        {
            _Endianness = endianness;
        }

        // Private Fields
        private Encoding _Encoding = Encoding.GetEncoding("Shift_JIS");
        private Endianness _Endianness;
        private byte[] _Bytes;

        // Accessors
        private byte[] Bytes
        {
            get { return _Bytes; }
            set
            {
                _Bytes = value;
                if (_Endianness == Endianness.Big)
                {
                    Array.Reverse(_Bytes);
                }
            }
        }
    }

    public enum Endianness
    {
        Big,
        Little
    }
}
