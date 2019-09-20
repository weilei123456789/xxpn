using DG.Tweening;
using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class Topic4Form : TopicBase
    {
        public override string[] EasyTopic
        {
            get
            {
                string[] _ti = new string[]
                {
                    string.Format("{0};{1}", "Easy2",    (int)SoundId.s4_easy_1),
                    string.Format("{0};{1}", "Easy3",    (int)SoundId.s4_easy_2),
                    string.Format("{0};{1}", "Easy4",    (int)SoundId.s4_easy_3),
                    string.Format("{0};{1}", "Easy5",    (int)SoundId.s4_easy_4),
                };
                return _ti;
            }
        }

        public override string[] TroubleTopic
        {
            get
            {
                string[] _ti = new string[]
                {
                    string.Format("{0};{1}", "Trouble1",    (int)SoundId.s4_diff_1 ),
                    string.Format("{0};{1}", "Trouble2",    (int)SoundId.s4_diff_2 ),
                    string.Format("{0};{1}", "Trouble3",    (int)SoundId.s4_diff_3 ),
                    string.Format("{0};{1}", "Trouble4",    (int)SoundId.s4_diff_4 ),
                    string.Format("{0};{1}", "Trouble5",    (int)SoundId.s4_diff_5 ),
                };
                return _ti;
            }
        }

        protected override bool m_IsUseMoviceBg
        {
            get
            {
                return true;
            }
        }

        [Header("----------------------------------------------")]
        [SerializeField]
        private Transform m_EasyTransform = null;
        [SerializeField]
        private Transform m_TroubleTransform = null;
        [SerializeField]
        private MapPuzzle[] m_EasyMapPuzzle = null;
        [SerializeField]
        private MapPuzzle[] m_TroubleMapPuzzle = null;

        private MapPuzzle m_CurSelectMapPuzzle = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.VideoPlayer.PlayLoadMovice("XXPN_EP01_sc002_light_BG_H");
            GameEntry.Sound.PlayMusic((int)MusicId.topic_4);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        protected override void OnLidarHitEvent(GameObject go, Vector3 vec)
        {
            if (m_ProduceingState != Produceing.Playing) return;
            TouchCondition(go);
            TouchTarget(go);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            switch (m_ProduceingState)
            {
                case Produceing.PanBai:
                    {
                        m_CurClipLength += elapseSeconds;
                        if (m_CurClipLength > m_ClipMaxLength)
                        {
                            Enter_Teaching();
                        }
                    }
                    break;
                case Produceing.Teaching:
                    {
                        Enter_PennySay();
                    }
                    break;
                case Produceing.PennySay:
                    {
                        m_CurClipLength += elapseSeconds;
                        if (m_CurClipLength > m_ClipMaxLength)
                        {
                            Enter_CountDown();
                        }
                    }
                    break;
                case Produceing.CountDown:
                    {
                        if (m_CountDown.IsComplate)
                        {
                            Enter_BrushTopic();
                        }
                    }
                    break;
                case Produceing.BrushTopic:
                    {
                        m_CurClipLength += elapseSeconds;
                        if (m_CurClipLength > m_ClipMaxLength)
                        {
                            m_CurClipLength = 0;
                            Enter_Playing();
                        }
                    }
                    break;

                case Produceing.Playing:
                    {
                        if (!StartPlay) return;
                        m_CurTime -= elapseSeconds;
                        m_TopicCale.SetTime((int)m_CurTime);
                        if (m_CurTime < 0)
                        {
                            m_CurTime = 0;
                            EnterFailed();
                        }
                    }
                    break;
                case Produceing.Success:
                    {
                        Enter_WaitNext();
                    }
                    break;
                case Produceing.Failed:
                    {
                        Enter_WaitNext();
                    }
                    break;
                case Produceing.WaitNext:
                    {
                        m_CurTime += elapseSeconds;
                        if (m_CurTime > 5f)
                        {
                            ResetData();
                            m_TopicCale.Small();
                            Enter_CountDown();
                        }
                    }
                    break;
            }
        }

        protected override void Enter_PanBai()
        {
            base.Enter_PanBai();
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_PanbaiSoundId);
            s_PanbaiSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s4_pangbai_1, ref m_ClipMaxLength);
        }

        protected override void Enter_Teaching()
        {
            base.Enter_Teaching();
        }

        protected override void Enter_PennySay()
        {
            base.Enter_PennySay();
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_PennySaySoundId);
            s_PennySaySoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s4_pangbai_2, ref m_ClipMaxLength);
        }

        protected override void Enter_PennySay_Trouble()
        {
            base.Enter_PennySay();
            // 蓝队也加入比赛，快看~他们来了
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_PennySaySoundId);
            s_PennySaySoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s4_pangbai_3, ref m_ClipMaxLength);
        }

        protected override void Enter_CountDown()
        {
            base.Enter_CountDown();
            m_CountDown.StartCountDown(null);
        }

        protected override void Enter_BrushTopic()
        {
            base.Enter_BrushTopic();
            if (m_DifficultyType == DifficultyType.Easy)
            {
                for (int i = 0; i < m_EasyMapPuzzle.Length; i++)
                {
                    if (m_EasyMapPuzzle[i].name.Contains(SelectTopicSplit[0]))
                    {
                        m_CurSelectMapPuzzle = m_EasyMapPuzzle[i];
                        m_CurSelectMapPuzzle.InitMapPuzzle(Enter_Success);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_TroubleMapPuzzle.Length; i++)
                {
                    if (m_TroubleMapPuzzle[i].name.Contains(SelectTopicSplit[0]))
                    {
                        m_CurSelectMapPuzzle = m_TroubleMapPuzzle[i];
                        m_CurSelectMapPuzzle.InitMapPuzzle(Enter_Success);
                        break;
                    }
                }
            }
            m_CurSoundId = int.Parse(SelectTopicSplit[1]);
            m_SuccessID = -1;
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_TopicSoundId);
            s_TopicSoundId = (int)GameEntry.Sound.PlaySoundAndLength(m_CurSoundId, ref m_ClipMaxLength);
        }

        protected override void Enter_WaitNext()
        {
            base.Enter_WaitNext();
        }

        //protected override string RandomTopic()
        //{
        //    if (m_DifficultyType == DifficultyType.Easy)
        //    {
        //        int index = Utility.Random.GetRandom(0, EasyTopic.Length);
        //        Log.Info(EasyTopic[index]);
        //        return EasyTopic[index];
        //    }
        //    else if (m_DifficultyType == DifficultyType.Trouble)
        //    {
        //        int index = Utility.Random.GetRandom(0, TroubleTopic.Length);
        //        Log.Info(TroubleTopic[index]);
        //        return TroubleTopic[index];
        //    }
        //    return "";
        //}

        protected override void InitBackGround(DifficultyType type)
        {
            base.InitBackGround(type);
            if (type == DifficultyType.Easy)
            {
                m_EasyTransform.gameObject.SetActive(true);
                m_TroubleTransform.gameObject.SetActive(false);
                Enter_PanBai();
                //Enter_CountDown();
            }
            else if (type == DifficultyType.Trouble)
            {
                m_EasyTransform.gameObject.SetActive(false);
                m_TroubleTransform.gameObject.SetActive(true);
                Enter_PennySay_Trouble();
            }
        }

        protected override void InitAnswer()
        {

        }

        /// <summary>
        /// open时候调用一次
        /// </summary>
        protected override void ResetData()
        {
            base.ResetData();
            if (m_CurSelectMapPuzzle != null)
            {
                m_CurSelectMapPuzzle.Clear();
                m_CurSelectMapPuzzle = null;
            }
        }

        private void TouchCondition(GameObject go)
        {
            if (go == null) return;
            if (m_CurSelectMapPuzzle != null)
            {
                m_CurSelectMapPuzzle.Select(go, null);
            }
        }

        private void TouchTarget(GameObject go)
        {
        }

    }

}