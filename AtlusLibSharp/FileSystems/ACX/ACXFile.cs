namespace AtlusLibSharp.FileSystems.ACX
{
    using Utilities;
    using System.IO;
    using IO;
    using System.Collections.Generic;

    public class ACXFile : BinaryFileBase
    {
        private uint _Size;
        private uint _Offset;
        private int _FileCount;
        public List<byte[]> _Data;
        
        public ACXFile()
        {
            _Data = new List<byte[]>();
        }

        public ACXFile(string path)
        {
            using (EndiannessReader reader = new EndiannessReader(File.OpenRead(path), Endianness.Big))
            {
                InternalRead(reader);
            }
        }

        public ACXFile(Stream stream)
        {
            using (EndiannessReader reader = new EndiannessReader(stream, Endianness.Big))
            {
                InternalRead(reader);
            }
        }
        
        private void InternalRead(EndiannessReader reader)
        {
            reader.ReadInt32();
            _FileCount = reader.ReadInt32();
            _Data = new List<byte[]>();

            for (int i = 0; i < _FileCount; i++)
            {
                _Offset = reader.ReadUInt32();
                _Size = reader.ReadUInt32();
                _Data.Add(reader.ReadBytesAtOffset((int)_Size, _Offset));
            }
        }

        internal override void InternalWrite(BinaryWriter writer)
        {
            using (EndiannessWriter endianWriter = new EndiannessWriter(writer.BaseStream, Endianness.Big))
            {
                endianWriter.Write(0);
                endianWriter.Write(_Data.Count);
                uint[] offsets = new uint[_Data.Count];
                uint[] sizes = new uint[_Data.Count];

                for (int i = 0; i < _Data.Count; i++)
                {
                    endianWriter.Write(0xDEADC0DE);
                    endianWriter.Write(0xDEADC0DE);
                }

                for (int i = 0; i < _Data.Count; i++)
                {
                    offsets[i] = (uint)endianWriter.GetPosition();
                    sizes[i] = (uint)_Data[i].Length;
                    endianWriter.Write(_Data[i]);
                    endianWriter.Write((byte)0);
                }

                endianWriter.SetPosition(0x8);
                for (int i = 0; i < _Data.Count; i++)
                {
                    endianWriter.Write(offsets[i]);
                    endianWriter.Write(sizes[i]);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    endianWriter.BaseStream.CopyTo(ms);
                    writer.Write(ms.ToArray());
                }
            }
        }
    }
}
