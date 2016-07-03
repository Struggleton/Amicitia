namespace AtlusLibSharp.Audio
{
    using Utilities;
    using System.IO;
    struct CriwareHeader
    {
        public const byte x80 = 0x80;
        //private const byte 0x00 = 0x00;
        public short CopyrightOffset;
        public byte EncodingType;
        public byte BlockSize;
        public byte SampleBitDepth;
        public byte ChannelCount;
        public int SampleRate;
        public int TotalSamples;
        public short HighpassFrequency;
        public byte Version;
        public byte Flags;
        public int Unknown;
        public int LoopEnabled;
        public int LoopBeginSampleIndex;
        public int LoopBeginByteIndex;
        public int LoopEndSampleIndex;
        public int LoopEndByteIndex;

        public CriwareHeader(EndiannessReader reader)
        {
            if (reader.ReadByte() != x80) throw new InvalidDataException("Invalid ADX file!");
            reader.ReadByte();
            CopyrightOffset = reader.ReadInt16();
            EncodingType = reader.ReadByte();
            BlockSize = reader.ReadByte();
            SampleBitDepth = reader.ReadByte();
            ChannelCount = reader.ReadByte();
            SampleRate = reader.ReadInt32();
            TotalSamples = reader.ReadInt32();
            HighpassFrequency = reader.ReadInt16();
            Version = reader.ReadByte();
            Flags = reader.ReadByte();
            Unknown = reader.ReadInt32();
            if (Version == 4) reader.ReadBytes(0xC);
            LoopEnabled = reader.ReadInt32();
            LoopBeginSampleIndex = reader.ReadInt32();
            LoopBeginByteIndex = reader.ReadInt32();
            LoopEndSampleIndex = reader.ReadInt32();
            LoopEndByteIndex = reader.ReadInt32();
        }
    }
}

