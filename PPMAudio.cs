
using PPMLib.Extensions;
using System;
using System.IO;
using NAudio.Wave;

namespace PPMLib
{
    public class PPMAudio
    {

        public _SoundHeader SoundHeader { get; set; }
        public _SoundData SoundData { get; set; }


        public PPMAudio()
        {
            SoundHeader = new _SoundHeader();
            SoundData = new _SoundData();
        }

        /// <summary>
        /// Returns the fully mixed audio of the Flipnote, Including its Sound Effects.
        /// Returns Null if no audio exists.
        /// </summary>
        /// <param name="flip"></param>
        /// <param name="sampleRate"></param>
        /// <returns>Signed 16-bit PCM audio</returns>
        public byte[] GetWavBGM(PPMFile flip, int sampleRate = 32768)
        {
            // start decoding
            AdpcmDecoder encoder = new AdpcmDecoder(flip);
            var decoded = encoder.getAudioMasterPcm(sampleRate);
            if(decoded.Length > 0)
            {
                byte[] output = new byte[decoded.Length];

                // thank you https://github.com/meemo
                for (int i = 0; i < decoded.Length; i += 2)
                {
                    try
                    {
                        output[i] = (byte)(decoded[i + 1] & 0xff);
                        output[i + 1] = (byte)(decoded[i] >> 8);
                    }
                    catch(Exception)
                    {

                    }
                    
                }
                var decodedDataStream = new MemoryStream(output);
                var s = new RawSourceWaveStream(decodedDataStream, new WaveFormat(sampleRate / 2, 16, 1));
                var outputDataStream = new MemoryStream();
                WaveFileWriter.WriteWavFileToStream(outputDataStream, s);
                
                return outputDataStream.ToArray();
            }
            return null;
        }

    }

    public class _SoundHeader
    {
        public uint BGMTrackSize;
        public uint SE1TrackSize;
        public uint SE2TrackSize;
        public uint SE3TrackSize;
        public byte CurrentFramespeed;
        public byte RecordingBGMFramespeed;
    }

    public class _SoundData
    {
        public byte[] RawBGM;
        public byte[] RawSE1;
        public byte[] RawSE2;
        public byte[] RawSE3;
    }
}
