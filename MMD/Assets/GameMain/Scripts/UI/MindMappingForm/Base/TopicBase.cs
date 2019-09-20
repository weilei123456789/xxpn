using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penny
{
    public abstract class TopicBase : UGuiForm
    {
        protected enum Produceing
        {
            None,
            // 旁白
            PanBai,
            // 教学
            Teaching,
            // pn说
            PennySay,
            // 倒计时
            CountDown,
            // 刷题
            BrushTopic,
            // 进行中
            Playing,
            // 正确
            Success,
            // 失败
            Failed,
            // next
            WaitNext,

        }

        public abstract string[] EasyTopic
        {
            get;
        }

        public abstract string[] TroubleTopic
        {
            get;
        }

        [SerializeField]
        private int m_AnswerTime = 30;
        [SerializeField]
        protected Controller m_Controller = null;
        [SerializeField]
        protected StudentRoot m_StudentRoot = null;
        [SerializeField]
        protected TopicCale m_TopicCale = null;
        [SerializeField]
        protected CountDown m_CountDown = null;
        [SerializeField]
        protected RawImage m_MoviceBackGround = null;
        [SerializeField]
        protected GuideBase m_Guide = null;

        protected Produceing m_ProduceingState = Produceing.None;
        protected DifficultyType m_DifficultyType = DifficultyType.None;
        protected DirType m_CurLeftRightDir = DirType.None;
        protected DirType m_CurUpDownDir = DirType.None;
        protected DirType m_CurFrontAfterDir = DirType.None;
        protected ColorType m_CurColor = ColorType.None;
        protected GraphicalType m_GraphicalType = GraphicalType.None;
        protected TeamType m_CurTeam = TeamType.None;


        
        /// <summary>
        /// 选择正确条件
        /// </summary>
        protected bool m_IsChooseSuccessCondition = false;
        // 选择正确目标
        protected bool m_IsChooseSuccessTarget = false;

        public static int m_ClipMaxLength = 0;
        protected float m_CurClipLength = 0;
        protected float m_CurTime = 0;
        protected int m_CurSoundId = 0;
        protected int m_SuccessID = 0;
        protected int m_CurTopicIndex = 0;
        protected static int s_TopicSoundId = -1;
        protected static int s_PanbaiSoundId = -1;
        protected static int s_PennySaySoundId = -1;

        protected abstract bool m_IsUseMoviceBg { get; }

        private int m_CalcStarNum = 0;

        private int m_SpeekAgainMaxLength = 0;
        private float m_SpeekAgainTime = 0;
        private bool m_IsCanSpeekAgain = false;
        /// <summary>
        /// 答题时间
        /// </summary>
        public int AnswerTime { get { return m_AnswerTime; } }
        // 选择的题目
        private string m_SelectTopic = string.Empty;
        private string[] m_SelectTopicSplit = null;
        public string[] SelectTopicSplit { get { return m_SelectTopicSplit; } }
        // 开始游戏
        private bool m_StartPlay = false;
        public bool StartPlay { get { return m_StartPlay; } }
        // 是否超时
        private bool m_IsTimeOut = false;
        public bool TimeOut { get { return m_IsTimeOut; } }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            if (m_IsUseMoviceBg)
                m_MoviceBackGround.texture = GameEntry.VideoPlayer.Texture;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_DifficultyType = DifficultyType.Easy;
            m_Controller.SpeekAgainCallBack += SpeekAgainCallBack;
            m_Controller.ResetCallBack += ResetCallBack;
            m_Controller.ChangeCallBack += ChangeCallBack;
            m_Controller.NextCallBack += NextCallBack;
            GameEntry.Windows.SubscribeUIWallEvent(OnLidarHitEvent);
            ResetData();
            InitBackGround(m_DifficultyType);
            InitAnswer();
        }

        protected override void OnClose(object userData)
        {
            if (m_Guide != null)
                m_Guide.Close();
            m_Controller.SpeekAgainCallBack -= SpeekAgainCallBack;
            m_Controller.ResetCallBack -= ResetCallBack;
            m_Controller.ChangeCallBack -= ChangeCallBack;
            m_Controller.NextCallBack -= NextCallBack;
            GameEntry.Windows.UnSubscribeUIWallEvent(OnLidarHitEvent);
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_ProduceingState == Produceing.Playing && m_StartPlay)
            {
                if ((int)m_CurTime <= m_SpeekAgainMaxLength) return;
                if (!m_IsCanSpeekAgain)
                {
                    m_SpeekAgainTime += elapseSeconds;
                    if (m_SpeekAgainTime > 15f)
                    {
                        if (!string.IsNullOrEmpty(m_SelectTopic))
                        {
                            GameEntry.Sound.StopSound(s_TopicSoundId);
                            s_TopicSoundId = (int)GameEntry.Sound.PlaySoundAndLength(m_CurSoundId, ref m_SpeekAgainMaxLength);
                        }
                        m_IsCanSpeekAgain = true;
                        m_SpeekAgainTime = 0;
                    }
                }
                else
                {
                    m_SpeekAgainTime += elapseSeconds;
                    if (m_SpeekAgainTime > m_SpeekAgainMaxLength)
                    {
                        m_IsCanSpeekAgain = false;
                        m_SpeekAgainTime = 0;
                    }
                }
            }
        }

        protected abstract void OnLidarHitEvent(GameObject go, Vector3 vec);

        protected virtual void SpeekAgainCallBack()
        {
            if (!string.IsNullOrEmpty(m_SelectTopic))
            {
                GameEntry.Sound.StopSound(s_TopicSoundId);
                s_TopicSoundId = (int)GameEntry.Sound.PlaySoundAndLength(m_CurSoundId, ref m_ClipMaxLength);
                GameEntry.Sound.PlayUISound((int)UISoundId.BtnSound);
            }
        }

        protected virtual void NextCallBack()
        {
            
            GameEntry.Sound.StopSound(s_TopicSoundId);
            GameEntry.Sound.StopSound(s_PanbaiSoundId);
            GameEntry.Sound.StopSound(s_PennySaySoundId);
            GameEntry.Sound.StopSound(TopicTouchBase.s_SoundSerialId);
            GameEntry.Sound.StopAllLoadedSounds();
            GameEntry.Sound.StopAllLoadingSounds();
            Close(true);
            ProcedureMindMapping.s_OpenFormSerialId = MindMappingManager.Instance.EnterNextLesson();
            GameEntry.Sound.PlayUISound((int)UISoundId.BtnSound);
        }

        protected virtual void ResetCallBack()
        {
            GameEntry.Sound.StopSound(s_TopicSoundId);
            GameEntry.Sound.StopSound(s_PanbaiSoundId);
            GameEntry.Sound.StopSound(s_PennySaySoundId);
            GameEntry.Sound.StopSound(TopicTouchBase.s_SoundSerialId);
            ResetData();
            m_StudentRoot.Clear();
            m_TopicCale.Small();
            //Enter_BrushTopic();
            Enter_CountDown();
            GameEntry.Sound.PlayUISound((int)UISoundId.BtnSound);
        }

        protected virtual void ChangeCallBack()
        {
            GameEntry.Sound.StopSound(s_TopicSoundId);
            GameEntry.Sound.StopSound(s_PanbaiSoundId);
            GameEntry.Sound.StopSound(s_PennySaySoundId);
            GameEntry.Sound.StopSound(TopicTouchBase.s_SoundSerialId);
            GameEntry.Sound.PlayUISound((int)UISoundId.MagicDis);
            //GameEntry.Sound.StopAllLoadedSounds();
            //GameEntry.Sound.StopAllLoadingSounds();
            if (m_DifficultyType == DifficultyType.Easy)
                m_DifficultyType = DifficultyType.Trouble;
            else if (m_DifficultyType == DifficultyType.Trouble)
                m_DifficultyType = DifficultyType.Easy;
            ResetData();
            m_TopicCale.Small();
            InitBackGround(m_DifficultyType);
            //Enter_BrushTopic();
            if (m_Guide != null)
                m_Guide.Close();
        }

        /// <summary>
        /// 随机一道题目
        /// </summary>
        /// <returns></returns>
        protected virtual string RandomTopic()
        {
            string[] Ti = (m_DifficultyType == DifficultyType.Easy) ? EasyTopic : TroubleTopic;
            int id = m_CurTopicIndex % Ti.Length;
            m_CurTopicIndex++;
            return Ti[id];
        }

        /// <summary>
        /// 初始化面板
        /// </summary>
        /// <param name="type"></param>
        protected virtual void InitBackGround(DifficultyType type)
        {
            m_CurTopicIndex = 0;
        }

        /// <summary>
        /// 初始化题目
        /// </summary>
        /// <param name="type"></param>
        protected abstract void InitAnswer();

        /// <summary>
        /// 重置数据,open时候调用一次
        /// </summary>
        protected virtual void ResetData()
        {
            m_ProduceingState = Produceing.None;
            m_SelectTopic = string.Empty;
            m_TopicCale.SetTime(m_AnswerTime, false);
            m_IsChooseSuccessCondition = false;
            m_IsChooseSuccessTarget = false;
            m_Istingting = true;
        }

        /// <summary>
        /// 进入旁白
        /// </summary>
        protected virtual void Enter_PanBai()
        {
            m_ProduceingState = Produceing.PanBai;
        }

        /// <summary>
        /// 进入教学
        /// </summary>
        protected virtual void Enter_Teaching()
        {
            m_ProduceingState = Produceing.Teaching;
            if (m_Guide != null)
                m_Guide.Open();
        }

        /// <summary>
        /// PennySay
        /// </summary>
        protected virtual void Enter_PennySay()
        {
            m_ProduceingState = Produceing.PennySay;
        }


        /// <summary>
        /// 进入旁白
        /// </summary>
        protected virtual void Enter_PennySay_Trouble()
        {
            m_ProduceingState = Produceing.PennySay;
        }


        /// <summary>
        /// CountDown
        /// </summary>
        protected virtual void Enter_CountDown()
        {
            m_ProduceingState = Produceing.CountDown;
        }

        /// <summary>
        /// BrushTopic
        /// </summary>
        protected virtual void Enter_BrushTopic()
        {
            m_ProduceingState = Produceing.BrushTopic;
            // 下一个学员
            m_StudentRoot.NextStudent();
            m_SelectTopic = RandomTopic();
            m_SelectTopicSplit = m_SelectTopic.Split(';');
        }

        /// <summary>
        /// Playing
        /// </summary>
        protected virtual void Enter_Playing()
        {
            m_ProduceingState = Produceing.Playing;
            m_StartPlay = true;
            m_IsCanSpeekAgain = false;
            m_CurTime = m_AnswerTime;
            m_TopicCale.SetTime(m_AnswerTime, false);
        }

        /// <summary>
        /// Success
        /// </summary>
        protected virtual void Enter_Success()
        {
            m_ProduceingState = Produceing.Success;
            m_StartPlay = false;
            m_IsTimeOut = false;
            
            GameEntry.Sound.PlayUISound((int)UISoundId.End);
            int spliteTime = m_AnswerTime / m_TopicCale.Length;
            int score = 0;
            m_CalcStarNum = 0;
            for (int i = 0; i < m_TopicCale.Length; i++)
            {
                if (m_CurTime > (spliteTime * i))
                {
                    m_CalcStarNum++;
                }
            }
            // 初始的那颗星为6分,剩余4个每颗星 + 1分,
            score = 6 + (m_CalcStarNum - 1) * 1;
            m_StudentRoot.SetScore(m_DifficultyType, score);
            GameEntry.Sound.StopSound(TopicTouchBase.s_SoundSerialId);
            GameEntry.Sound.PlayUISound((int)UISoundId.Success);
            ///答题成功有分数和星星声音
            m_Istingting = true;
            m_IsUpScore = true;
        }

        /// <summary>
        /// Failed
        /// </summary>
        protected virtual void EnterFailed()
        {
            m_ProduceingState = Produceing.Failed;
            m_StartPlay = false;
            m_IsTimeOut = true;
            // 超时失败,不得分,0颗星
            m_CalcStarNum = 0;
            m_StudentRoot.SetScore(m_DifficultyType, 0);
            GameEntry.Sound.StopSound(TopicTouchBase.s_SoundSerialId);
            GameEntry.Sound.PlayUISound((int)UISoundId.Failed);
            ///答题失败没有分数和星星声音
            m_Istingting = false;
            m_IsUpScore = false;
        }

        /// <summary>
        /// WaitNext
        /// </summary>
        protected virtual void Enter_WaitNext()
        {
            m_ProduceingState = Produceing.WaitNext;
            m_CurTime = 0;
            m_TopicCale.Bigger(m_StudentRoot, m_CalcStarNum);
            //完成游戏之后的操作
            //完成答题涨星星的叮叮声
            StartCoroutine(IEnumeratorNext());
            StartCoroutine(IEnumeratorUpScore());
        }
       protected bool m_Istingting = true;
        protected IEnumerator IEnumeratorNext()
        {
            if (m_Istingting)
            {
                m_Istingting = false;
                for (int i = 0; i < m_CalcStarNum; i++)
                {
                    GameEntry.Sound.PlayUISound((int)UISoundId.tingting);
                    yield return new WaitForSeconds(0.5f);
                }
            } 
        }
        /// <summary>
        /// 指定次数播放指定间隔时间的指定声音
        /// </summary>
        /// <returns></returns>
        protected IEnumerator IEnumeratorSoundShake(int count,int soundID,float interval)
        {
            for (int i = 0; i < count; i++)
            {
                GameEntry.Sound.PlayUISound(soundID);
                yield return new WaitForSeconds(interval);
            }

        }

       protected bool m_IsUpScore = true;
        protected IEnumerator  IEnumeratorUpScore()
        {
            yield return new WaitForSeconds(0.5f * m_CalcStarNum);
            if (m_IsUpScore)
            {
                m_IsUpScore = false;
                GameEntry.Sound.PlayUISound((int)UISoundId.UpScore);
            }
        }



    }

}