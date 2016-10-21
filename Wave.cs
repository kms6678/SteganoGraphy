using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.IO;

namespace steganography
{
    public class Wave
    {
        // string filePath;
        const string ChunkID = "RIFF";
        const string Format = "WAVE";
        const string Subchunk1ID = "fmt ";
        const string Subchunk2ID = "data";
        const int PCM = 1;
        public const int SampleUnit = 1024;
        bool successToOpen = false;
        //Stream waveFileStream;
        //Offset    size
        string chunkId;     //0         4
        uint chunkSize;     //4         4       전체파일 크기
        string format;      //8         4
        string subchunk1ID; //12        4
        uint subchunk1Size; //16        4
        ushort audioFormat;   //20        2
        ushort numChannels;   //22        2
        uint sampleRate = 8000;    //24        4  
        uint byteRate;      //28        4       == SampleRate * NumChannels * BitsPerSample/8
        ushort blockAlign;    //32        2       == NumChannels * BitsPerSample/8
        ushort bitsPerSample; //34        2       8 bits = 8, 16 bits = 16, etc.
        string subchunk2ID;   //36        4
        uint subchunk2Size; //40        4       == NumSamples * NumChannels * BitsPerSample/8

        uint playtime = 0;
        uint sampleDataSize = 0;

        public byte[] sampleData = null;
        public byte[] valied = null;

        const int chunkDescriptorSize = 12;
        const int subchunk2HeadSize = 8;
        private string fileName;

        private Stream pcmStream = new MemoryStream();

        public uint TotalLength
        {
            get { return (this.subchunk2Size - 8) / this.blockAlign; }
        }

        public uint NumChannels
        {
            get { return this.numChannels; }
        }

        public uint SampleRate
        {
            get { return this.sampleRate; }
        }

        public bool SuccessToOpen
        {
            get { return successToOpen; }
        }

        public bool isStreo()
        {
            if (this.numChannels == 2)
                return true;
            return false;
        }

        public uint BlockAlign
        {
            get { return this.blockAlign; }
        }

        public uint BitsPerSample
        {
            get { return this.bitsPerSample; }
        }

        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        //오픈시 헤드를 읽음

        public Wave(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            this.pcmStream.Write(buffer, 0, (int)stream.Length);
            this.pcmStream.Seek(0, SeekOrigin.Begin);

            /////////////////////////////////

            // The "RIFF" chunk descriptor //

            /////////////////////////////////

            this.chunkId = readAscii4Byte(this.pcmStream);
            if (Wave.ChunkID != this.chunkId)
            {
                this.pcmStream.Close();
                //successToOpen = false;
            }

            BinaryReader br = new BinaryReader(this.pcmStream);
            this.chunkSize = br.ReadUInt32();
            this.format = readAscii4Byte(this.pcmStream);
            if (Wave.Format != this.format)
            {
                this.pcmStream.Close();
                return;
            }

            /////////////////////////

            // The "fmt" sub-chunk //

            /////////////////////////

            this.subchunk1ID = readAscii4Byte(this.pcmStream);
            if (this.subchunk1ID != Wave.Subchunk1ID)
            {
                this.pcmStream.Close();
                return;
            }

            this.subchunk1Size = br.ReadUInt32();
            this.audioFormat = br.ReadUInt16();
            this.numChannels = br.ReadUInt16();
            this.sampleRate = br.ReadUInt32();
            this.byteRate = br.ReadUInt32();
            this.blockAlign = br.ReadUInt16();
            this.bitsPerSample = br.ReadUInt16();

            //부가 정보가 있을 경우 넘김
            if (16 != this.subchunk1Size)
            {
                br.ReadBytes((int)this.subchunk1Size - 16);
            }

            //////////////////////////

            // The "data" sub-chunk //

            //////////////////////////

            this.subchunk2ID = readAscii4Byte(this.pcmStream);
            if (this.subchunk2ID != Wave.Subchunk2ID)
            {
                this.pcmStream.Close();
                return;
            }
            this.subchunk2Size = br.ReadUInt32();
            this.successToOpen = true;

            playtime = subchunk2Size / sampleRate;
            sampleDataSize = playtime * sampleRate * numChannels * bitsPerSample / 8;

            sampleData = new byte[sampleDataSize - 2];

            valied = new byte[2];
            valied = br.ReadBytes(2);

            sampleData = br.ReadBytes(sampleData.Length); //샘플데이터 바이트배열로 읽어들이기 완료!!!!!

            return;
        }

        private string readAscii4Byte(Stream fs)
        {
            byte[] buf = new byte[5];
            fs.Read(buf, 0, 4);
            buf[4] = 0;
            ASCIIEncoding asciiEn = new ASCIIEncoding();

            return asciiEn.GetString(buf, 0, 4);
        }

        public void Save_wavfile(byte[] buffer)
        {
            Microsoft.Win32.SaveFileDialog sfdFile = new Microsoft.Win32.SaveFileDialog();

            if (sfdFile.ShowDialog() == true)
            {
                BinaryWriter f_out = new BinaryWriter(new FileStream(sfdFile.FileName, FileMode.Create));

                //binarywriter로 string형식을 쓰니까 첫번째 바이트에 string의 길이바이트가 같이 붙여져서 제대로된 PCM형식을 쓸수 없어서 16진수(리틀엔디언방식)로 직접입력

                f_out.Write(0x46464952); //리틀엔디언으로 씀 RIFF
                f_out.Write(chunkSize);
                f_out.Write(0x45564157); //WAVE
                f_out.Write(0x20746d66); //fmt
                f_out.Write(subchunk1Size);
                f_out.Write(audioFormat);
                f_out.Write(numChannels);
                f_out.Write(sampleRate);
                f_out.Write(byteRate);
                f_out.Write(blockAlign);
                f_out.Write(bitsPerSample);
                f_out.Write(0x61746164); //data
                f_out.Write(subchunk2Size);
                //PCM헤더 작성 완료.

                for (int i = 0; i < 2; i++)
                {
                    valied[i] = 0x99;
                }
                f_out.Write(valied);
                //valied가 0x9999를 가지면 데이터하이딩이 완료된 이미지.

                f_out.Write(sampleData);
                f_out.Close();
            }
        }

        static int w_location = 0; //데이터를 추가할때 데이터 위치

        public void Write_LSB_bit(byte[] buffer, int value)
        {
            byte temp = 0;

            if (value == 0) //0을 저장할때 원래 데이터가 0일때는 넘어띄어야 함.
            {
                temp = (byte)(buffer[w_location] & 0x01);
                if (temp == 0)
                {
                    w_location = w_location + 1;
                    return;
                }
                buffer[w_location] = (byte)(buffer[w_location] ^ 1 << 0);
            }
            if (value == 1) //1을 저장할때 원래 데이터가 1일때는 넘어띄어야 함.
            {
                temp = (byte)(buffer[w_location] & 0x01);
                if (temp == 1)
                {
                    w_location = w_location + 1;
                    return;
                }
                buffer[w_location] = (byte)(buffer[w_location] ^ 1 << 0);
            }

            w_location = w_location + 1;
        }

        public void Write_End(byte[] buffer) //샘플데이터의 수정을 완료하고나서 수정의 끝을 알리기위해 0x00바이트를 삽입...(음원데이터내에서 0x00데이터는 존재하지 않는다.)
        {
            buffer[w_location] = 0x00;
        }

        int Find_End(byte[] buffer) //마지막으로 수정된 바이트위치 찾아서 추가된 글자의 숫자를 알아내기 위한 함수.
        {
            int result = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0x00)
                    result = i;
            }

            return result;
        }

        static int r_location = 0; //추출할떄 데이터 위치

        public string Extracted_LSB_bit(byte[] buffer)
        {
            int j = 0;
            int end_location = Find_End(buffer);
            int charcount = (end_location - r_location) / 8;

            char final = ' ';

            string result = string.Empty;
            string hidden = string.Empty;

            while (j++ < charcount)
            {
                for (int i = 0; i < 8; i++)
                {
                    byte temp = (byte)(buffer[r_location] & 1 << 0);
                    hidden = hidden + temp.ToString();
                    r_location = r_location + 1;
                }
                final = BinaryToChar(hidden);
                result = result + final.ToString();
                hidden = string.Empty;
            }

            return result;
        }

        public char BinaryToChar(string str)
        {
            double temp = 0;
            char result_value = ' ';

            for (int i = 7; i >= 0; i--)
            {
                if (str.ElementAt(i).Equals('1'))
                    temp = temp + Math.Pow(2, 7 - i);
            }

            result_value = (char)temp;

            return result_value;
        }
    }
}