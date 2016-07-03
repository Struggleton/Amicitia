namespace AtlusLibSharp.FileSystems.AWB
{
    using System.IO;
    using Utilities;
    using System.Collections.Generic;
    using IO;
    using System;
    using System.Text;

    public class AWBFile : BinaryFileBase
    {
        private const string MAGIC = "AFS2";
        private int Verson;
        private int FileCount;
        private int BaseAlignValue;
        private short[] Alignments;
        public List<byte[]> Data;
        public string name = "";

        public AWBFile()
        {
            Data = new List<byte[]>();
        }

        public AWBFile(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                InternalRead(reader);
            }
        }

        public AWBFile(string path)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
            {
                InternalRead(reader);
            }
        }

        public void InternalRead(BinaryReader reader)
        {
            Data = new List<byte[]>();
            if (reader.ReadCString(4) != MAGIC) throw new InvalidDataException("Not a valid AWB file!");
            Verson = reader.ReadInt32();
            FileCount = reader.ReadInt32();
            BaseAlignValue = reader.ReadInt32();
            Alignments = reader.ReadInt16Array(FileCount);

            int Offset = reader.ReadInt32();
            int CurrentAlignment;
            for (int i = 0; i < FileCount; i++)
            {
                int NextOffset = reader.ReadInt32();
                if (BaseAlignValue != 0) CurrentAlignment = BaseAlignValue;
                else CurrentAlignment = Alignments[i];
                Offset = AlignmentHelper.Align(Offset, CurrentAlignment);
                int Size = NextOffset - Offset;
                Data.Add(reader.ReadBytesAtOffset(Size, Offset));
                Offset = NextOffset;
            }
        }

        internal override void InternalWrite(BinaryWriter writer)
        {
            int[] Offsets = new int[Data.Count];
            writer.Write(Encoding.ASCII.GetBytes(MAGIC));
            writer.Write(0x20401);
            writer.Write(Data.Count);
            writer.Write(32);
            for (int i = 0; i < Data.Count; i++)
            {
                writer.Write((short)i);
            }
            long OffsetsOffset = writer.GetPosition();
            for (int i = 0; i < Data.Count; i++)
                writer.Write(0);
            for (int i = 0; i < Data.Count; i++)
            {
                int FinalPosition = (int)(AlignmentHelper.Align(writer.GetPosition(), 32) - writer.GetPosition());
                Offsets[i] = (int)AlignmentHelper.Align(writer.GetPosition(), 32) - FinalPosition + ((i == 0) ? 4 : 0);
                writer.AlignPosition(32);
                writer.Write(Data[i]);
            }
            writer.SetPosition(OffsetsOffset);
            writer.Write(Offsets);
        }
    }
}
