using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Threading;
using UnityGameFramework.Runtime;

namespace Penny
{
    /// <summary>
    /// speeker朗读者枚举常量
    /// </summary>
    public enum Speeker
    {
        小燕_青年女声_中英文_普通话 = 0,
        小宇_青年男声_中英文_普通话,
    }

    public enum SynthStatus
    {
        TTS_FLAG_STILL_HAVE_DATA = 1,
        TTS_FLAG_DATA_END,
        TTS_FLAG_CMD_CANCELED
    }

    public class XunFeiTemplate
    {
        public class JinDuEventArgs : EventArgs
        {
            public readonly int AllLenth;
            public readonly int AllP;
            public readonly int ThisLenth;
            public readonly int ThisP;
            public JinDuEventArgs(int allLenth, int allp, int thisLenth, int thisp)
            {
                AllLenth = allLenth;
                AllP = allp;
                ThisLenth = thisLenth;
                ThisP = thisp;
            }
        }

        public event EventHandler<JinDuEventArgs> Finished;

        /// <summary>
        /// 引入TTSDll函数的类
        /// </summary>
        private class TTSDll
        {
            [DllImport("msc_x64", CallingConvention = CallingConvention.StdCall)]
            public static extern int MSPLogin(string usr, string pwd, string parameters);

            [DllImport("msc_x64", CallingConvention = CallingConvention.Winapi)]
            public static extern int MSPLogout();

            [DllImport("msc_x64", CallingConvention = CallingConvention.Winapi)]
            public static extern IntPtr QTTSSessionBegin(string _params, ref int errorCode);

            [DllImport("msc_x64", CallingConvention = CallingConvention.Winapi)]
            public static extern int QTTSTextPut(string sessionID, string textString, uint textLen, string _params);

            [DllImport("msc_x64", CallingConvention = CallingConvention.Winapi)]
            public static extern IntPtr QTTSAudioGet(string sessionID, ref int audioLen, ref SynthStatus synthStatus, ref int errorCode);

            [DllImport("msc_x64", CallingConvention = CallingConvention.Winapi)]
            public static extern int QTTSSessionEnd(string sessionID, string hints);

            [DllImport("msc_x64", CallingConvention = CallingConvention.Winapi)]
            public static extern int QTTSGetParam(string sessionID, string paramName, string paramValue, ref uint valueLen);
        }

        private string sessionID;

        private string _speed;

        private string _vol;

        private string _speeker;

        private Dictionary<Speeker, string> DSpeeker = new Dictionary<Speeker, string>();

        /// <summary>
        /// 构造函数，初始化引擎
        /// </summary>
        /// <param name="configs">初始化引擎参数</param>
        /// <param name="szParams">开始会话用参数</param>
        public XunFeiTemplate(string name, string password, string configs)
        {
            DSpeeker.Add(Speeker.小燕_青年女声_中英文_普通话, "xiaoyan");
            DSpeeker.Add(Speeker.小宇_青年男声_中英文_普通话, "xiaoyu");

            int ret = TTSDll.MSPLogin(name, password, configs);
            if (ret != 0) throw new Exception("初始化TTS引擎错误，错误代码：" + ret);

            _speed = "50";
            _vol = "100";
            _speeker = "xiaoyan";

            Log.Info(TTSPath(_speeker));
        }

        public void SetSpeaker(Speeker speeker)
        {
            if (DSpeeker.ContainsKey(speeker))
                _speeker = DSpeeker[speeker];
        }

        public void CloseXunFei()
        {
            int ret = TTSDll.MSPLogout();
            if (ret != 0) throw new Exception("逆初始化TTS引擎错误，错误代码：" + ret);
        }

        public MemoryStream SpeechSynthesis(string SpeekText)
        {
            MemoryStream mStream = new MemoryStream(1024 * 8);

            mStream.Write(new byte[44], 0, 44);

            Speek(SpeekText, ref mStream);

            //int ret = TTSDll.MSPLogout();
            //if (ret != 0) throw new Exception("逆初始化TTS引擎错误，错误代码：" + ret);

            WAVE_Header header = getWave_Header((int)mStream.Length - 44);     //创建wav文件头
            byte[] headerByte = StructToBytes(header);                         //把文件头结构转化为字节数组                      //写入文件头
            mStream.Position = 0;                                                        //定位到文件头
            mStream.Write(headerByte, 0, headerByte.Length);                             //写入文件头
            return mStream;
        }

        public string TTSPath(string speeker)
        {
            //fo|C:\\Users\\Administrator\\Desktop\\tts\\xiaofeng.jet;fo|C:\\Users\\Administrator\\Desktop\\tts\\common.jet
            string path = UnityEngine.Application.streamingAssetsPath;
            //Log.Info(path);
            path = path.Replace("/", "\\");
            string combine = "fo|{0}\\tts\\{1}.jet;fo|{2}\\tts\\common.jet";

            return string.Format(combine, path, speeker, path);
        }

        /// <summary>
        /// 把文本转换成声音，写入指定的内存流
        /// </summary>
        /// <param name="SpeekText">要转化成语音的文字</param>
        /// <param name="mStream">合成结果输出的音频流</param>
        private void Speek(string SpeekText, ref MemoryStream mStream)
        {
            if (SpeekText == "" || _speed == "" || _vol == "" || _speeker == "") return;
            //string szParams = "ssm=1," + _speeker + ",spd=" + _speed + ",aue=speex-wb;7,vol=" + _vol + ",auf=audio/L16;rate=16000";

            string szParams = "engine_type = local, voice_name = " + _speeker + ", text_encoding = UTF8, tts_res_path = " + TTSPath(_speeker) + ", sample_rate = 16000, speed = " + _speed + ", volume = " + _vol + ", pitch = 50, rdn = 2";
            //string szParams = "engine_type = cloud, voice_name = " + _speeker + ", text_encoding = UTF8, sample_rate = 16000, speed = " + _speed + ", volume = " + _vol + ", pitch = 50, rdn = 2";
            //string szParams = "engine_type = cloud ,voice_name = " + _speeker + ", text_encoding = GB2312,sample_rate = 16000";

            int ret = 0;
            try
            {
                sessionID = Marshal.PtrToStringAnsi(TTSDll.QTTSSessionBegin(szParams, ref ret));
                if (ret != 0)
                {
                    UnityEngine.Debug.Log(ret);
                    throw new Exception("初始化TTS引会话错误，错误代码：" + ret);
                }
                ret = TTSDll.QTTSTextPut(sessionID, SpeekText, (uint)Encoding.Default.GetByteCount(SpeekText), null);
                if (ret != 0)
                {
                    UnityEngine.Debug.Log(ret);
                    throw new Exception("向服务器发送数据，错误代码：" + ret);
                }//IntPtr audio_data;
                SynthStatus synth_status = SynthStatus.TTS_FLAG_STILL_HAVE_DATA;

                while (true)
                {
                    int audio_len = 0;
                    IntPtr source = TTSDll.QTTSAudioGet(sessionID, ref audio_len, ref synth_status, ref ret);
                    byte[] array = new byte[audio_len];
                    if (audio_len > 0)
                    {
                        Marshal.Copy(source, array, 0, audio_len);
                    }
                    mStream.Write(array, 0, audio_len);//将合成的音频字节数据存放到内存流中
                    //Thread.Sleep(15);//防止CPU频繁占用
                    if (synth_status == SynthStatus.TTS_FLAG_DATA_END || ret != 0)
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message);
            }
            finally
            {
                ret = TTSDll.QTTSSessionEnd(sessionID, "");
                if (ret != 0) throw new Exception("结束TTS会话错误，错误代码：" + ret);
            }
        }

        /// <summary>
        /// 把文字转化为声音,单路配置，一种语音
        /// </summary>
        /// <param name="speekText">要转化成语音的文字</param>
        /// <param name="outWaveFlie">把声音转为文件，默认为不生产wave文件</param>
        private void speek(string speekText, string outWaveFlie = null)
        {
            if (speekText == "" || _speed == "" || _vol == "" || _speeker == "") return;
            string szParams = "ssm=1," + _speeker + ",spd=" + _speed + ",aue=speex-wb;7,vol=" + _vol + ",auf=audio/L16;rate=16000";
            int ret = 0;
            try
            {
                sessionID = Ptr2Str(TTSDll.QTTSSessionBegin(szParams, ref ret));
                if (ret != 0) throw new Exception("初始化TTS引会话错误，错误代码：" + ret);

                ret = TTSDll.QTTSTextPut(sessionID, speekText, (uint)Encoding.Default.GetByteCount(speekText), string.Empty);
                if (ret != 0) throw new Exception("向服务器发送数据，错误代码：" + ret);
                IntPtr audio_data;
                int audio_len = 0;
                SynthStatus synth_status = SynthStatus.TTS_FLAG_STILL_HAVE_DATA;

                MemoryStream fs = new MemoryStream();
                fs.Write(new byte[44], 0, 44);                              //写44字节的空文件头

                while (synth_status == SynthStatus.TTS_FLAG_STILL_HAVE_DATA)
                {
                    audio_data = TTSDll.QTTSAudioGet(sessionID, ref audio_len, ref synth_status, ref ret);
                    if (ret != 0) break;
                    byte[] data = new byte[audio_len];
                    if (audio_len > 0) Marshal.Copy(audio_data, data, 0, audio_len);
                    fs.Write(data, 0, data.Length);
                }

                WAVE_Header header = getWave_Header((int)fs.Length - 44);     //创建wav文件头
                byte[] headerByte = StructToBytes(header);                         //把文件头结构转化为字节数组                      //写入文件头
                fs.Position = 0;                                                        //定位到文件头
                fs.Write(headerByte, 0, headerByte.Length);                             //写入文件头

                fs.Position = 0;
                System.Media.SoundPlayer pl = new System.Media.SoundPlayer(fs);
                pl.Stop();
                pl.Play();
                if (outWaveFlie != null)
                {
                    FileStream ofs = new FileStream(outWaveFlie, FileMode.Create);
                    fs.WriteTo(ofs);
                    fs.Close();
                    ofs.Close();
                    fs = null;
                    ofs = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                ret = TTSDll.QTTSSessionEnd(sessionID, "");
                if (ret != 0) throw new Exception("结束TTS会话错误，错误代码：" + ret);
            }
        }

        /// <summary>
        /// wave文件头
        /// </summary>
        private struct WAVE_Header
        {
            public int RIFF_ID;           //4 byte , 'RIFF'
            public int File_Size;         //4 byte , 文件长度
            public int RIFF_Type;         //4 byte , 'WAVE'

            public int FMT_ID;            //4 byte , 'fmt'
            public int FMT_Size;          //4 byte , 数值为16或18，18则最后又附加信息
            public short FMT_Tag;          //2 byte , 编码方式，一般为0x0001
            public ushort FMT_Channel;     //2 byte , 声道数目，1--单声道；2--双声道
            public int FMT_SamplesPerSec;//4 byte , 采样频率
            public int AvgBytesPerSec;   //4 byte , 每秒所需字节数,记录每秒的数据量
            public ushort BlockAlign;      //2 byte , 数据块对齐单位(每个采样需要的字节数)
            public ushort BitsPerSample;   //2 byte , 每个采样需要的bit数

            public int DATA_ID;           //4 byte , 'data'
            public int DATA_Size;         //4 byte , 
        }

        /// <summary>
        /// 根据数据段的长度，生产文件头
        /// </summary>
        /// <param name="data_len">音频数据长度</param>
        /// <returns>返回wav文件头结构体</returns>
        WAVE_Header getWave_Header(int data_len)
        {
            WAVE_Header wav_Header = new WAVE_Header();
            wav_Header.RIFF_ID = 0x46464952;        //字符RIFF
            wav_Header.File_Size = data_len + 36;
            wav_Header.RIFF_Type = 0x45564157;      //字符WAVE

            wav_Header.FMT_ID = 0x20746D66;         //字符fmt
            wav_Header.FMT_Size = 16;
            wav_Header.FMT_Tag = 0x0001;
            wav_Header.FMT_Channel = 1;             //单声道
            wav_Header.FMT_SamplesPerSec = 16000;   //采样频率
            wav_Header.AvgBytesPerSec = 32000;      //每秒所需字节数
            wav_Header.BlockAlign = 2;              //每个采样1个字节
            wav_Header.BitsPerSample = 16;           //每个采样8bit

            wav_Header.DATA_ID = 0x61746164;        //字符data
            wav_Header.DATA_Size = data_len;

            return wav_Header;
        }

        /// <summary>
        /// 把结构体转化为字节序列
        /// </summary>
        /// <param name="structure">被转化的结构体</param>
        /// <returns>返回字节序列</returns>
        Byte[] StructToBytes(Object structure)
        {
            Int32 size = Marshal.SizeOf(structure);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                Byte[] bytes = new Byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// 指针转字符串
        /// </summary>
        /// <param name="p">指向非托管代码字符串的指针</param>
        /// <returns>返回指针指向的字符串</returns>
        public static string Ptr2Str(IntPtr p)
        {
            List<byte> lb = new List<byte>();
            while (Marshal.ReadByte(p) != 0)
            {
                lb.Add(Marshal.ReadByte(p));
                p = p + 1;
            }
            byte[] bs = lb.ToArray();
            return Encoding.Default.GetString(bs);
        }
    }
}