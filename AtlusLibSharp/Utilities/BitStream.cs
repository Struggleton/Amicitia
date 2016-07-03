namespace AtlusLibSharp.Utilities
{
    using System;
    using System.IO;
    public class BitStream
    {
        public BitStream(Stream input)  { BaseStream = ((MemoryStream)input).ToArray(); }
        public BitStream(short input)   {  BaseStream = BitConverter.GetBytes(input); }
        public BitStream(int input)     {  BaseStream = BitConverter.GetBytes(input); }
        public BitStream(long input)    {  BaseStream = BitConverter.GetBytes(input); }
        public BitStream(ushort input)  {  BaseStream = BitConverter.GetBytes(input); }
        public BitStream(uint input)    {  BaseStream = BitConverter.GetBytes(input); }
        public BitStream(ulong input)   {  BaseStream = BitConverter.GetBytes(input); }
        public BitStream(byte input)    {  BaseStream = BitConverter.GetBytes(input); }
        public BitStream(char input)    { BaseStream = BitConverter.GetBytes(input);  }

        private byte[] _BaseStream;
        public byte[] BaseStream
        {
            get { return _BaseStream; }
            set
            {
                _BaseStream = value;
                _Length = _BaseStream.Length;
                Position = 0;
                _BitPosition = 0;
            }
        }

        private long _Length;
        public long Length
        {
            get { return _Length; }
            private set { _Length = value; }
        }

        private long _Position = 0;
        public long Position
        {
            get { return _Position;  }
            set { Position = value; }
        }

        private byte _BitPosition = 0;
        public byte BitPosition
        {
            get { return _BitPosition; }
            set { _BitPosition = value; }
        }

        private byte _CurrentByte;
        public byte CurrentByte
        {
            get { return _CurrentByte; }
            private set { _CurrentByte = value; }
        }

        public byte ReadNibble()
        {
            return ReadBits(4);
        }

        public byte ReadBit()
        {
            return ReadBits(1);
        }

        public byte ReadBits(int Count)
        {
            if (BitPosition == 8) _BitPosition = 0; Position += 1;
            if (BitPosition + Count > 8 || Position + Count > Length) throw new IndexOutOfRangeException("Read past amount of bits in current byte!");
            if (BitPosition == 0) CurrentByte = BaseStream[Position];
            return (byte)((CurrentByte & (Count << BitPosition)) >> BitPosition);
        }

        public void Seek(SeekTypes type, long value)
        {
            switch (type)
            {
                case SeekTypes.Absolute:
                    if (value > 8) throw new EndOfStreamException("End of bitstream past!");
                    Position = 8;
                    break;
                case SeekTypes.Current:
                    if (value + Position > 8) throw new EndOfStreamException("End of bitstream past!");
                    Position += value;
                    break;
                case SeekTypes.End:
                    if (Position - value < -1) throw new EndOfStreamException("End of bitstream past!");
                    Position -= value;
                    break;
            }
        }
        
        public void BitSeek(SeekTypes type, byte value)
        {
            switch (type)
            {
                case SeekTypes.Absolute:
                    if (value > 8) throw new EndOfStreamException("End of bitstream past!");
                    BitPosition = 8;
                    break;
                case SeekTypes.Current:
                    if (value + BitPosition > 8) throw new EndOfStreamException("End of bitstream past!");
                    BitPosition += value;
                    break;
                case SeekTypes.End:
                    if (BitPosition - value < -1) throw new EndOfStreamException("End of bitstream past!");
                    BitPosition -= value;
                    break;
            }
        }
    }

    public enum SeekTypes
    {
        Absolute, Current, End
    }
}
