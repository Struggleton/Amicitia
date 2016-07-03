namespace AtlusLibSharp.FileSystems.ISO
{
    using Utilities;
    using System.IO;
    using IO;
    using System.Collections.Generic;

    public class ISOFile
    {
        private const int _ReservedArea = 0x8000;
        private byte _Type;
        private const string _Identifier = "CD001";
        private const int _Version = 0x1;

        public ISOFile()
        {
            
        }

        public void InternalRead(EndiannessReader reader)
        {
            reader.ReadBytes(_ReservedArea);
            _Type = reader.ReadByte();
            if (reader.ReadCString(5) != _Identifier || reader.ReadByte() != _Version)
                throw new InvalidDataException("Invalid CVM file!");
            switch(_Type)
            {
                case (byte)VolumeDescriptorTypes.PrimaryVolume:
                    PrimaryDescriptor iso = new PrimaryDescriptor(reader);
                    break;
                case (byte)VolumeDescriptorTypes.VolumeTerminator:
                case (byte)VolumeDescriptorTypes.BootRecord:
                case (byte)VolumeDescriptorTypes.SupplementaryVolume:
                case (byte)VolumeDescriptorTypes.VolumePartition:
                    reader.ReadBytes(0x7F9); // skip because not supported or does not matter
                    break;
            }
        }

        enum VolumeDescriptorTypes : byte
        {
            BootRecord = 0,
            PrimaryVolume = 1,
            SupplementaryVolume = 2,
            VolumePartition = 3,
            VolumeTerminator = 255
        };
    }
}
