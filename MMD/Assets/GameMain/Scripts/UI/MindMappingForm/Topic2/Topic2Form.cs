using DG.Tweening;
using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class Topic2Form : TopicBase
    {
        public override string[] EasyTopic
        {
            get
            {
                string[] _ti = new string[]
                {
                    string.Format("{0};{1};{2};{3};{4}", TeamType.RedTeam, DirType.After, ColorType.Red,    3, (int)SoundId.s2_easy_1),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.RedTeam, DirType.Front, ColorType.Yellow, 2, (int)SoundId.s2_easy_2),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.RedTeam, DirType.Front, ColorType.Black,  5, (int)SoundId.s2_easy_3),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.RedTeam, DirType.After, ColorType.Green,  4, (int)SoundId.s2_easy_4),
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
                    string.Format("{0};{1};{2};{3};{4}", TeamType.BlueTeam, DirType.After, ColorType.Purple, 2, (int)SoundId.s2_diff_1 ),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.RedTeam,  DirType.After, ColorType.Blue,   5, (int)SoundId.s2_diff_2 ),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.RedTeam,  DirType.Front, ColorType.Yellow, 3, (int)SoundId.s2_diff_3 ),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.BlueTeam, DirType.Front, ColorType.Green,  4, (int)SoundId.s2_diff_4 ),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.RedTeam,  DirType.Front, ColorType.Yellow, 6, (int)SoundId.s2_diff_5),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.BlueTeam, DirType.After, ColorType.Red,    4, (int)SoundId.s2_diff_6),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.RedTeam,  DirType.Front, ColorType.Blue,   5, (int)SoundId.s2_diff_7),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.BlueTeam, DirType.Front, ColorType.Black,  1, (int)SoundId.s2_diff_8),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.RedTeam,  DirType.Front, ColorType.Purple, 4, (int)SoundId.s2_diff_9),
                    string.Format("{0};{1};{2};{3};{4}", TeamType.BlueTeam, DirType.Front, ColorType.Green,  4, (int)SoundId.s2_diff_10),
                };
                return _ti;
            }
        }

        protected override bool m_IsUseMoviceBg
        {
            get
            {
                return false;
            }
        }

        [Header("------------------------------------------")]
        [SerializeField]
        private Transform m_EasyTransform = null;
        [SerializeField]
        private Transform m_EasyFlagTransform = null;
        [SerializeField]
        private Transform m_TroubleTransform = null;
        [SerializeField]
        private Transform m_TroubleFlagTransform = null;

        [SerializeField]
        private Car[] m_EasyCars = null;
        [SerializeField]
        private Car[] m_TroubleCars = null;
        [SerializeField]
        private PaintDrum[] m_PaintDrums = null;

        private PaintDrum m_SelectPaintDrums = null;

        private Vector3 m_EasyPosition = Vector3.zero;
        private Vector3 m_TroublePosition = Vector3.zero;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_EasyPosition = m_EasyTransform.localPosition;
            m_TroublePosition = m_TroubleTransform.localPosition;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.VideoPlayer.PlayLoadMovice("XXPN_EP01_sc002_light_BG_H");
            GameEntry.Sound.PlayMusic((int)MusicId.topic_2);
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
                        //if (m_Guide.IsComplateGuide)
                        {
                            Enter_PennySay();
                            //m_Guide.ClearPoint();
                        }
                    }
                    break;
                case Produceing.PennySay:
                    {
                        m_CurClipLength += elapseSeconds;
                        if (m_CurClipLength > m_ClipMaxLength)
                        {
                            //m_PennyAnimator.SetInteger("state", 0);
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
                        ////完成游戏之后的操作
                        ////完成答题涨星星的叮叮声
                        //StartCoroutine(IEnumeratorNext());
                        //StartCoroutine(IEnumeratorUpScore());

                        m_CurTime += elapseSeconds;
                        if (m_CurTime > 5f)
                        {
                            ResetData();
                            m_TopicCale.Small();
                            //Enter_BrushTopic();
                            Enter_CountDown();
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// 欢呼后进入游戏
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected IEnumerator IEnumeratorHail(DifficultyType type)
        {
            GameEntry.Sound.PlayUISound((int)UISoundId.Hail);
            
            yield return new WaitForSeconds(2);
            if (type == DifficultyType.Easy)
            {
                Enter_PanBai();

            }
            else if (type == DifficultyType.Trouble)
            {
                Enter_PennySay_Trouble();
            }

        }

        protected override void Enter_PanBai()
        {
            base.Enter_PanBai();
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_PanbaiSoundId);
            s_PanbaiSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s2_pangbai1, ref m_ClipMaxLength);
        }

        protected override void Enter_Teaching()
        {
            base.Enter_Teaching();
            //PaintDrum first = null;
            //for (int i = 0; i < m_PaintDrums.Length; i++)
            //{
            //    m_PaintDrums[i].Scale();
            //    if (m_DrawerProps[i].PropType == PropType.Snacks && first == null)
            //    {
            //        first = m_DrawerProps[i];
            //    }
            //}
            //Drawer second = null;
            //for (int i = 0; i < m_DrawerTouch_L.Length; i++)
            //{
            //    if (m_DrawerTouch_L[i].Id == 5)
            //    {
            //        second = m_DrawerTouch_L[i];
            //        break;
            //    }
            //}
            //m_Guide.StartGuide(first.transform, second.transform);
        }

        protected override void Enter_PennySay()
        {
            base.Enter_PennySay();
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_PennySaySoundId);
            s_PennySaySoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s2_pangbai2, ref m_ClipMaxLength);
        }

        protected override void Enter_PennySay_Trouble()
        {
            base.Enter_PennySay();
            // 蓝队也加入比赛，快看~他们来了
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_PennySaySoundId);
            s_PennySaySoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s2_pangbai3, ref m_ClipMaxLength);
        }

        protected override void Enter_CountDown()
        {
            base.Enter_CountDown();
            m_CountDown.StartCountDown(null);
        }

        protected override void Enter_BrushTopic()
        {
            base.Enter_BrushTopic();
            m_CurTeam = (TeamType)Enum.Parse(typeof(TeamType), SelectTopicSplit[0]);
            m_CurFrontAfterDir = (DirType)Enum.Parse(typeof(DirType), SelectTopicSplit[1]);
            m_CurColor = (ColorType)Enum.Parse(typeof(ColorType), SelectTopicSplit[2]);
            if (DirType.Front == m_CurFrontAfterDir)
            {
                m_SuccessID = int.Parse(SelectTopicSplit[3]);
            }
            else if (DirType.After == m_CurFrontAfterDir)
            {
                m_SuccessID = 6 - int.Parse(SelectTopicSplit[3]) + 1;
            }
            m_CurSoundId = int.Parse(SelectTopicSplit[4]);
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_TopicSoundId);
            s_TopicSoundId = (int)GameEntry.Sound.PlaySoundAndLength(m_CurSoundId, ref m_ClipMaxLength);

            //if (m_DifficultyType == DifficultyType.Easy)
            //{
            //    m_EasyTransform.DOLocalMove(m_EasyPosition, 3f).SetEase(Ease.Linear);
            //}
            //else if (m_DifficultyType == DifficultyType.Trouble)
            //{
            //    m_TroubleTransform.DOLocalMove(m_TroublePosition, 3f).SetEase(Ease.Linear);
            //}
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
            int wallWeight = GameEntry.WindowsConfig.Config.Screen_Wall_Width;
            GameEntry.Sound.PlayUISound((int)UISoundId.GunFire);
            GameEntry.Sound.PlayUISound((int)UISoundId.CarIn);
            if (type == DifficultyType.Easy)
            {
                m_EasyTransform.gameObject.SetActive(true);
                m_EasyFlagTransform.gameObject.SetActive(true);
                m_TroubleTransform.gameObject.SetActive(false);
                m_TroubleFlagTransform.gameObject.SetActive(false);

                Vector3 move = m_EasyPosition + Vector3.right * wallWeight;
                m_EasyTransform.transform.localPosition = move;
                
                m_EasyTransform.DOLocalMove(m_EasyPosition, 3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    StartCoroutine(IEnumeratorHail(type));
                   // Enter_PanBai();
                });
            }
            else if (type == DifficultyType.Trouble)
            {
                m_EasyTransform.gameObject.SetActive(false);
                m_EasyFlagTransform.gameObject.SetActive(false);
                m_TroubleTransform.gameObject.SetActive(true);
                m_TroubleFlagTransform.gameObject.SetActive(true);

                Vector3 move = m_TroublePosition + Vector3.right * wallWeight;
                m_TroubleTransform.transform.localPosition = move;

                m_TroubleTransform.DOLocalMove(m_TroublePosition, 3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    StartCoroutine(IEnumeratorHail(type));
                   // Enter_PennySay_Trouble();
                });
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
            for (int i = 0; i < m_EasyCars.Length; i++)
            {
                m_EasyCars[i].StopAutoScale();
                m_EasyCars[i].Clear();
            }
            for (int i = 0; i < m_TroubleCars.Length; i++)
            {
                m_TroubleCars[i].StopAutoScale();
                m_TroubleCars[i].Clear();
            }
            for (int i = 0; i < m_PaintDrums.Length; i++)
            {
                m_PaintDrums[i].Clear();
            }
            m_SelectPaintDrums = null;

            //int wallWeight = GameEntry.WindowsConfig.Config.Screen_Wall_Width;
            //if (m_DifficultyType == DifficultyType.Easy)
            //{
            //    Vector3 move = m_EasyPosition + Vector3.right * wallWeight;
            //    m_EasyTransform.transform.localPosition = move;
            //}
            //else if (m_DifficultyType == DifficultyType.Trouble)
            //{
            //    Vector3 move = m_TroublePosition + Vector3.right * wallWeight;
            //    m_TroubleTransform.transform.localPosition = move;
            //}
        }

        private void TouchCondition(GameObject go)
        {
            if (go == null) return;
            if (m_IsChooseSuccessCondition) return;
            for (int i = 0; i < m_PaintDrums.Length; i++)
            {
                if (m_PaintDrums[i].TouchObj == go)
                {
                    m_IsChooseSuccessCondition = m_PaintDrums[i].Select(m_PaintDrums[i].PropType, (int)m_CurColor);
                    if (m_IsChooseSuccessCondition)
                    {
                        m_SelectPaintDrums = m_PaintDrums[i];
                        for (int j = 0; j < m_PaintDrums.Length; j++)
                        {
                            if (m_PaintDrums[j].TouchObj != go)
                            {
                                m_PaintDrums[j].Gary();
                            }
                        }
                    }
                    break;
                }
            }
        }

        private void TouchTarget(GameObject go)
        {
            if (go == null) return;
            if (!m_IsChooseSuccessCondition) return;
            if (m_IsChooseSuccessTarget) return;

            if (m_DifficultyType == DifficultyType.Easy)
            {
                for (int i = 0; i < m_EasyCars.Length; i++)
                {
                    if (m_EasyCars[i].TouchObj == go)
                    {
                        m_IsChooseSuccessTarget = m_EasyCars[i].Select(m_EasyCars[i].PropType,
                            m_EasyCars[i].Team,
                            m_SuccessID,
                            m_SelectPaintDrums,
                            Enter_Success);
                        if (m_IsChooseSuccessTarget)
                        {
                            for (int j = 0; j < m_EasyCars.Length; j++)
                            {
                                if (m_EasyCars[j].TouchObj != go)
                                {
                                    m_EasyCars[j].Clear();
                                }
                            }
                        }
                        break;
                    }
                }
            }
            else if (m_DifficultyType == DifficultyType.Trouble)
            {
                for (int i = 0; i < m_TroubleCars.Length; i++)
                {
                    if (m_TroubleCars[i].TouchObj == go)
                    {
                        m_IsChooseSuccessTarget = m_TroubleCars[i].Select(m_TroubleCars[i].PropType,
                            m_TroubleCars[i].Team,
                            m_SuccessID,
                            m_SelectPaintDrums,
                            Enter_Success);
                        if (m_IsChooseSuccessTarget)
                        {
                            for (int j = 0; j < m_TroubleCars.Length; j++)
                            {
                                if (m_TroubleCars[j].TouchObj != go)
                                {
                                    m_TroubleCars[j].Clear();
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

    }
}