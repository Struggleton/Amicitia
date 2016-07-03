namespace AtlusLibSharp.FileSystems.ISO
{
    using Utilities;
    using System;
    using System.IO;

    public class PrimaryDescriptor
    {
        private string _SystemIdentifier;
        private string _VolumeIdentifier;
        private uint _VolumeSpaceSize;
        private ushort _VolumeSetSize;
        private ushort _VolumeSequenceNumber;
        private ushort _LogicalBlockSize;
        private uint _PathTableSize;
        private uint _LocationOfTypeLPathTable;
        private uint _LocationOfOptionalTypeLPathTable;
        private uint _LocationOfTypeMPathTable;
        private uint _LocationOfOptionalTypeMPathTable;
        private byte[] _DirectoryEntryRoot;
        private string _VolumeSetIdentifier;
        private string _PublisherIdentifier;
        private string _DataPreparerIdentifier;
        private string _ApplicationIdentifier;
        private string _CopyrightFileIdentifier;
        private string _AbstractFileIdentifier;
        private string _BibliographicFileIdentifier;
        private DateTime _VolumeCreationDate;
        private DateTime _VolumeModificationDate;
        private DateTime _VolumeExpirationDate;
        private DateTime _VolumeEffectiveDate;
        private const ushort _FileStructureVersion = 0x1;

        public PrimaryDescriptor(EndiannessReader reader)
        {
            reader.SetEndianness(Endianness.Little);
            reader.ReadByte();
            _SystemIdentifier = reader.ReadCString(0x20).TrimEnd(' ');
            _VolumeIdentifier = reader.ReadCString(0x20).TrimEnd(' ');
            reader.ReadBytes(0x8);
            _VolumeSpaceSize = ReadInt32LittleBig(reader);
            reader.ReadBytes(0x20);
            _VolumeSetSize = ReadInt16LittleBig(reader);
            _VolumeSequenceNumber = ReadInt16LittleBig(reader);
            _LogicalBlockSize = ReadInt16LittleBig(reader);
            _PathTableSize = ReadInt32LittleBig(reader);
            _LocationOfTypeLPathTable = reader.ReadUInt32();
            _LocationOfOptionalTypeLPathTable = reader.ReadUInt32();
            reader.SetEndianness(Endianness.Big);
            _LocationOfTypeMPathTable = reader.ReadUInt32();
            _LocationOfOptionalTypeMPathTable = reader.ReadUInt32();
            _DirectoryEntryRoot = reader.ReadBytes(0x22);
            _VolumeSetIdentifier = reader.ReadCString(0x80).TrimEnd(' ');
            _PublisherIdentifier = reader.ReadCString(0x80).TrimEnd(' ');
            _DataPreparerIdentifier = reader.ReadCString(0x80).TrimEnd(' ');
            _ApplicationIdentifier = reader.ReadCString(0x80).TrimEnd(' ');
            _CopyrightFileIdentifier = reader.ReadCString(0x26).TrimEnd(' ');
            _AbstractFileIdentifier = reader.ReadCString(0x24).TrimEnd(' ');
            _BibliographicFileIdentifier = reader.ReadCString(0x25).TrimEnd(' ');
            _VolumeCreationDate = ReadTime(reader);
            _VolumeModificationDate = ReadTime(reader);
            _VolumeExpirationDate = ReadTime(reader);
            _VolumeEffectiveDate = ReadTime(reader);
            if (reader.ReadByte() != _FileStructureVersion)
                throw new InvalidDataException("Invaluid FileStructureVersion!");
            reader.ReadByte();
            reader.ReadBytes(0x200);
            reader.ReadBytes(0x28D);
        }

        public ushort ReadInt16LittleBig(EndiannessReader reader)
        {
            reader.SetEndianness(Endianness.Big);
            reader.ReadUInt16();
            return reader.ReadUInt16();
        }

        public uint ReadInt32LittleBig(EndiannessReader reader)
        {
            reader.SetEndianness(Endianness.Big);
            reader.ReadUInt32();
            return reader.ReadUInt32();
        }

        private DateTime ReadTime(EndiannessReader reader)
        {
            DateTime time = new DateTime();
            time.AddYears(Convert.ToInt32(reader.ReadCString(4)));
            time.AddMonths(Convert.ToInt16(reader.ReadCString(2)));
            time.AddDays(Convert.ToInt16(reader.ReadCString(2)));
            time.AddHours(Convert.ToInt16(reader.ReadCString(2)));
            time.AddMinutes(Convert.ToInt16(reader.ReadCString(2)));
            time.AddSeconds(Convert.ToInt16(reader.ReadCString(2)));
            time.AddMilliseconds(Convert.ToInt16(reader.ReadCString(2)));
            reader.ReadByte();
            return time;
        }
    }
}
