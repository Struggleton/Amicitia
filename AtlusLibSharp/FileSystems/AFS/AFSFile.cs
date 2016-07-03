namespace AtlusLibSharp.FileSystems.AFS
{
    using System.Collections.Generic;
    using Utilities;
    using System.IO;
    using IO;
    using System.Text;

    public class AFSFile : BinaryFileBase
    {
        private const int BLOCKSIZE = 0x800;
        private const string MAGIC = "AFS";
        private byte Null;
        private int FileCount;
        private int[] Offsets;
        private int[] Sizes;
        private int MetaOffset;
        private int MetaSize;
        public string[] Names;
        public List<byte[]> Data = new List<byte[]>();
        private List<byte[]> TimeStamps = new List<byte[]>();

        public AFSFile()
        {
            Data = new List<byte[]>();
            TimeStamps = new List<byte[]>();
        }

        public AFSFile(Stream stream)
        {
            InternalRead(stream);
        }

        public AFSFile(string path)
        {
            using (FileStream stream = File.OpenRead(path)) InternalRead(stream);
        }

        private void InternalRead(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                Data = new List<byte[]>();
                TimeStamps = new List<byte[]>();

                if (reader.ReadCString(3) != MAGIC) throw new InvalidDataException("Invalid AFS data!");
                Null = reader.ReadByte();
                FileCount = reader.ReadInt32();
                Offsets = new int[FileCount];
                Sizes = new int[FileCount];
                for (int i = 0; i < FileCount; i++)
                {
                    Offsets[i] = reader.ReadInt32();
                    Sizes[i] = reader.ReadInt32();
                }

                MetaOffset = reader.ReadInt32();
                MetaSize = reader.ReadInt32();
                for (int i = 0; i < FileCount; i++)
                {
                    Data.Add(reader.ReadBytesAtOffset(Sizes[i], Offsets[i]));
                }

                reader.SetPosition(MetaOffset);
                Names = new string[FileCount];
                for (int i = 0; i < FileCount; i++)
                {
                    Names[i] = reader.ReadCString(32);
                    TimeStamps.Add(reader.ReadBytes(16));
                }
            }
        }

        internal override void InternalWrite(BinaryWriter writer)
        {
            writer.Write(Encoding.ASCII.GetBytes(MAGIC));
            writer.Write((byte)0);
            writer.Write(Data.Count);
            for (int i = 0; i < Data.Count; i++)
            {
                writer.Write(0xDEADC0DE);
                writer.Write(0xDEADC0DE);
            }

            writer.Write(0xDEADC0DE);
            writer.Write(0xDEADC0DE);

            Offsets = new int[Data.Count];
            Sizes = new int[Data.Count];
            for (int i = 0; i < Data.Count; i++)
            {
                WritePadding(writer, BLOCKSIZE);
                Offsets[i] = (int)writer.GetPosition();
                Sizes[i] = Data[i].Length;
                writer.Write(Data[i]);
            }

            MetaOffset = (int)writer.GetPosition();
            for (int i = 0; i < Data.Count; i++)
            {
                writer.WriteCString(Names[i], 32);
                writer.Write(new byte[16]);
            }

            MetaSize = (int)writer.BaseStream.Position - MetaOffset;
            WritePadding(writer, BLOCKSIZE);

            writer.SetPosition(8);
            for (int i = 0; i < Data.Count; i++)
            {
                writer.Write(Offsets[i]);
                writer.Write(Sizes[i]);
            }

            writer.Write(MetaOffset);
            writer.Write(MetaSize);
        }

        private void WritePadding(BinaryWriter writer, int BlockSize)
        {
            while (writer.BaseStream.Length % BlockSize != 0) { writer.Write((byte)0); }
        }
    }
}
