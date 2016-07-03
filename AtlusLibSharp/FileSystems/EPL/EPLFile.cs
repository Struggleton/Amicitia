namespace AtlusLibSharp.FileSystems.EPL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using IO;
    using System.IO;
    using Utilities;

    public class EPLFile //: BinaryFileBase
    {
        public string[] Names;
        private int FileCount;
        private int Unk;
        private int DataStart;
        private int[] TableOffsets;
        private int Offset;
        private int Size;
        public List<byte[]> Data;
        
        public EPLFile()
        {
            Data = new List<byte[]>();
        }

        public EPLFile(Stream stream)
        {
            InternalRead(stream);
        }

        public EPLFile(string path)
        {
            using (FileStream stream = File.OpenRead(path))
                InternalRead(stream);
        }

        public void InternalRead(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.SetPosition(0x80);
                FileCount = reader.ReadInt32();
                Unk = reader.ReadInt32();
                DataStart = reader.ReadInt32();

                reader.SetPosition(DataStart);
                Names = new string[FileCount];
                TableOffsets = new int[FileCount];
                Data = new List<byte[]>();
                for (int i = 0; i < FileCount; i++)
                {
                    reader.ReadBytes(0x90);
                    TableOffsets[i] = reader.ReadInt32();
                    reader.ReadBytes(8);
                    Names[i] = reader.ReadCString(36);
                }

                for (int i = 0; i < FileCount; i++)
                {
                    reader.SetPosition(TableOffsets[i] + 0x20);
                    Offset = reader.ReadInt32() + TableOffsets[i];
                    Size = reader.ReadInt32();
                    Data.Add(reader.ReadBytesAtOffset(Size, Offset));
                }
            }
        }

        /*
        internal override void InternalWrite(BinaryWriter writer)
        {
            WriteDummyData(writer);
            writer.SetPosition(0x80);
            writer.Write(Data.Count);
            writer.Write(0);
            writer.Write(0x90);
            writer.Write(0);
            WriteDummyData(writer);
            for (int i = 0; i < Data.Count; i++)
            {
                writer.Write(0); // Table Offset 
                writer.Write((long)0);
                writer.Write(Encoding.ASCII.GetBytes(Names[i].PadRight(36, '\x00')));
                if (i != Data.Count) WriteDummyData(writer);
            }
        }

        private void WriteDummyData(BinaryWriter writer)
        {
            byte[] Header = new byte[] {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F,
                0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0xA0, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F,
                0xFF, 0xFF, 0xFF, 0xFF, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00 };
            writer.Write(Header);
        }
        */
    }
}
