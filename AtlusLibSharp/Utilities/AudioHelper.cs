namespace AtlusLibSharp.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Media;

    /// <summary>
    /// Assists with basic Audio functions.
    /// </summary>
    public static class AudioHelper
    {

        /// <summary>
        /// Returns an array of PCM samples from a wav file.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static dynamic GetSamplesWAV(Stream stream)
        {
            using (EndiannessReader reader = new EndiannessReader(stream, Endianness.Little))
            {
                if (reader.ReadCString(4) != "RIFF") throw new InvalidDataException("Invalid WAV file!");
                int _ChunkSize = reader.ReadInt32();
                if (reader.ReadCString(8) != "WAVEfmt ") throw new InvalidDataException("Invalid WAV file!");
                int FMTSize = reader.ReadInt32();
                short AudioFormat = reader.ReadInt16();
                short NumChannels = reader.ReadInt16();
                int SampleRate = reader.ReadInt32();
                int ByteRate = reader.ReadInt32();
                short BlockAlign = reader.ReadInt16();
                short BitsPerSample = reader.ReadInt16();
                if (reader.ReadCString(4) != "DATA") throw new InvalidDataException("Invalid WAV file!");
                int DataSize = reader.ReadInt32();

                List<int> Samples = new List<int>();
                for (int i = 0; i < DataSize / BlockAlign; i++)
                {
                    if (BitsPerSample == 8) Samples.Add(reader.ReadByte());
                    if (BitsPerSample == 16) Samples.Add(reader.ReadInt16());
                    if (BitsPerSample == 32) Samples.Add(reader.ReadInt32());
                }

                if (BitsPerSample == 8) return Samples.Cast<byte>().ToArray();
                if (BitsPerSample == 16) return Samples.Cast<int>().ToArray();
                return Samples;
            }
        }

        /// <summary>
        /// Writes interleaved PCM samples to the provided Stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="SampleRate"></param>
        /// <param name="Samples"></param>
        public static Stream WriteSamplesWAV(int SampleRate, short[] Samples)
        {
            EndiannessWriter writer = new EndiannessWriter(new MemoryStream());
            writer.Write(Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(0);
            writer.Write(Encoding.ASCII.GetBytes("WAVEfmt "));
            writer.Write(16);
            writer.Write((short)1);
            writer.Write((short)2);
            writer.Write(SampleRate);
            writer.Write(4 * SampleRate);
            writer.Write((short)4);
            writer.Write((short)16);
            writer.Write(Encoding.ASCII.GetBytes("data"));
            writer.Write(Samples.Length);
            writer.Write(Samples);
            writer.SetPosition(4);
            writer.Write((int)writer.GetLength());
            return writer.BaseStream;

        }

        /// <summary>
        /// Plays interleaved PCM samples while returning a SoundPlayer.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="Samples"></param>
        /// <param name="SampleRate"></param>
        public static SoundPlayer PlaySamples(short[] Samples, int Channels, int SampleRate)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                SoundPlayer OutPlayer = new SoundPlayer(WriteSamplesWAV(SampleRate, Samples));
                OutPlayer.Play();
                try { return OutPlayer; }
                finally {  }
            }
        }

        /// <summary>
        /// Stops playing the SoundPlayer provided.
        /// </summary>
        /// <param name="player"></param>
        public static void StopPlaying(SoundPlayer player)
        {
            player.Stop();
        }

        /// <summary>
        /// Continues playing the SoundPlayer provided.
        /// </summary>
        /// <param name="player"></param>
        public static void ContinuePlaying(SoundPlayer player)
        {
            player.Play();
        }

        /// <summary>
        /// Clamps a value to a value within the provided ranges.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}
