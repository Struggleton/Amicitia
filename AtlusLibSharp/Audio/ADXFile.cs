namespace AtlusLibSharp.Audio
{
    using System;
    using System.IO;
    using Utilities;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    public class ADXFile
    {
        private const int BASEVOL = 0x4000;

        public ADXFile(Stream stream)
        {
            using (EndiannessReader reader = new EndiannessReader(stream, Endianness.Big))
            {
                using (EndiannessWriter writer = new EndiannessWriter(File.Open("_DEBUG_.WAV", FileMode.Create), Endianness.Little))
                {
                    CriwareHeader Header = new CriwareHeader(reader);
                    reader.SetPosition(Header.CopyrightOffset + 4);

                    double z, a, b, c;
                    z = Math.Cos(2.0 * Math.PI * Header.HighpassFrequency / Header.SampleRate);
                    a = Math.Sqrt(2) - z;
                    b = Math.Sqrt(2) - 1.0;
                    c = (a - Math.Sqrt((a + b) * (a - b))) / b;
                    var coef1 = Math.Floor(c * 8192);
                    var coef2 = Math.Floor(c * c * -4096);

                    int SampleSets = 0;
                    short[] Samples = new short[32];
                    List<short> SamplesList = new List<short>();
                    PreviousSamples[] previous = new PreviousSamples[2];

                    previous[0].s1 = 0; previous[1].s2 = 0;
                    previous[0].s1 = 0; previous[1].s2 = 0;
                    if (Header.ChannelCount == 1)
                    {
                        while (SampleSets < Header.TotalSamples / 32)
                        {
                            ConvertSample(Samples, reader.ReadBytes(18), previous[0], coef1, coef2);
                            SamplesList.AddRange(Samples);
                            SampleSets++;
                        }
                    }

                    if (Header.ChannelCount == 2)
                    {
                        while (SampleSets < Header.TotalSamples / 64)
                        {
                            ConvertSample(Samples, reader.ReadBytes(18), previous[0], coef1, coef2);
                            SamplesList.AddRange(Samples);
                            ConvertSample(Samples, reader.ReadBytes(18), previous[1], coef1, coef2);
                            SamplesList.AddRange(Samples);
                            SampleSets++;
                        }
                    }

                    Stream data = AudioHelper.WriteSamplesWAV(Header.SampleRate, Samples.ToArray());
                    FileStream file = new FileStream("test.wav", FileMode.Create);
                    data.CopyTo(file);
                }
            }
        }

        private void ConvertSample(short[] Out, byte[] In, PreviousSamples Previous, double Coef1, double Coef2)
        {
            using (EndiannessReader reader = new EndiannessReader(new MemoryStream(In)))
            {
                int Current = 0;
                short Scale = reader.ReadInt16();
                short s1 = Previous.s1;
                short s2 = Previous.s2;

                for (int i = 0; i < 16; i++)
                {
                    sbyte Sample = reader.ReadSByte();
                    sbyte SampleTrunc = (sbyte)(Sample >> 4);
                    if ((SampleTrunc & 8) == 1) SampleTrunc -= 16;
                    int s0 = SampleTrunc * Scale + (((int)Math.Floor(Coef1) * s1 + (int)Math.Floor(Coef2) * s2) >> 12);
                    short NewSample = (short)AudioHelper.Clamp(s0, -32768, 32767);
                    Out[Current++] = NewSample;
                    s2 = s1;
                    s1 = NewSample;

                    
                    SampleTrunc = (sbyte)(SampleTrunc & 15);
                    if ((SampleTrunc & 8) == 1) SampleTrunc -= 16;
                    s0 = SampleTrunc * Scale + (((int)Math.Floor(Coef1) * s1 + (int)Math.Floor(Coef2) * s2) >> 12);
                    NewSample = (short)AudioHelper.Clamp(s0, -32768, 32767);
                    Out[Current++] = NewSample;
                    s2 = s1;
                    s1 = NewSample;
                }
                Previous.s1 = s1;
                Previous.s2 = s2;
            }
        }

        private struct PreviousSamples
        {
            public short s1, s2;
        }
    }
}