using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;
using GameFramework;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Penny
{
    /// <summary>
    /// 道具id: 玩具0 衣服1 书2 零食3
    /// </summary>
    public class TopicOneForm : TopicBase
    {
        public override string[] EasyTopic
        {
            get
            {
                string[] _ti = new string[]
                {
                    string.Format("{0};{1};{2};{3};{4}",Enum.GetName(typeof(PropType), PropType.Toys), Enum.GetName(typeof(DirType), DirType.Left), Enum.GetName(typeof(DirType), DirType.Up),2, (int)SoundId.s1_easy_1),
                    string.Format("{0};{1};{2};{3};{4}",Enum.GetName(typeof(PropType), PropType.Cothes), Enum.GetName(typeof(DirType), DirType.Left), Enum.GetName(typeof(DirType), DirType.Down),3,(int)SoundId.s1_easy_2),
                    string.Format("{0};{1};{2};{3};{4}",Enum.GetName(typeof(PropType), PropType.Book), Enum.GetName(typeof(DirType), DirType.Left), Enum.GetName(typeof(DirType), DirType.Down),2,(int)SoundId.s1_easy_3),
                    string.Format("{0};{1};{2};{3};{4}",Enum.GetName(typeof(PropType), PropType.Snacks), Enum.GetName(typeof(DirType), DirType.Left), Enum.GetName(typeof(DirType), DirType.Up),5,(int)SoundId.s1_easy_4),
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
                    string.Format("{0};{1};{2};{3};{4}",Enum.GetName(typeof(PropType), PropType.Toys), Enum.GetName(typeof(DirType), DirType.Right), Enum.GetName(typeof(DirType), DirType.Up),5,(int)SoundId.s1_diff_1),
                    string.Format("{0};{1};{2};{3};{4}",Enum.GetName(typeof(PropType), PropType.Cothes), Enum.GetName(typeof(DirType), DirType.Left), Enum.GetName(typeof(DirType), DirType.Down),2,(int)SoundId.s1_diff_2),
                    string.Format("{0};{1};{2};{3};{4}",Enum.GetName(typeof(PropType), PropType.Book), Enum.GetName(typeof(DirType), DirType.Left), Enum.GetName(typeof(DirType), DirType.Up),2,(int)SoundId.s1_diff_3),
                    string.Format("{0};{1};{2};{3};{4}",Enum.GetName(typeof(PropType), PropType.Snacks), Enum.GetName(typeof(DirType), DirType.Right), Enum.GetName(typeof(DirType), DirType.Down),3,(int)SoundId.s1_diff_4),
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

        [Header("-----------------------------------------------")]
        [SerializeField]
        private Transform m_LeftBan = null;
        [SerializeField]
        private Transform m_LeftBan_2 = null;
        [SerializeField]
        private Transform m_RightBan = null;
        [SerializeField]
        private Transform m_PropTrs = null;
        [SerializeField]
        private DrawerProp[] m_DrawerProps = null;
        [SerializeField]
        private Drawer[] m_DrawerTouch_L = null;
        [SerializeField]
        private Drawer[] m_DrawerTouch_L_2 = null;
        [SerializeField]
        private Drawer[] m_DrawerTouch_R = null;
        [SerializeField]
        private Sprite[] m_ToysSprites = null;
        [SerializeField]
        private Sprite[] m_CothesSprites = null;
        [SerializeField]
        private Sprite[] m_BookSprites = null;
        [SerializeField]
        private Sprite[] m_SnacksSprites = null;
        [SerializeField]
        private Animator m_PennyAnimator = null;
        [SerializeField]
        private UISpriteAnimation m_BoomEffect = null;

        private bool m_IsFristMove = true;
        private DrawerProp m_SelectProps = null;
        private Drawer m_SelectDrawer = null;
        private PropType m_CurPropType = PropType.None;
        private PropType[] m_NewProps;
        private Vector3 m_InitPos = Vector3.zero;
        private bool m_IsTips = false;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_InitPos = m_PennyAnimator.transform.localPosition;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //GameEntry.VideoPlayer.PlayLoadMovice("XXPN_EP01_sc002_light_BG_H");
            GameEntry.Sound.PlayMusic((int)MusicId.topic_1);
            InitPenny();
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
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
                        if (m_Guide.IsComplateGuide)
                        {
                            Enter_PennySay();
                            for (int i = 0; i < m_DrawerProps.Length; i++)
                            {
                                m_DrawerProps[i].StopAutoScale();
                                m_DrawerProps[i].ClearScale();
                                m_DrawerProps[i].ClearColor();
                            }
                        }
                    }
                    break;
                case Produceing.PennySay:
                    {
                        m_CurClipLength += elapseSeconds;
                        if (m_CurClipLength > m_ClipMaxLength)
                        {
                            m_PennyAnimator.SetInteger("state", 0);
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
                        //m_TimeText.text = ((int)m_CurTime).ToString();
                        m_TopicCale.SetTime((int)m_CurTime);
                        Tips();
                        RandomJump();

                        if (m_ClipMaxLength>0)
                        {
                            m_CurClipLength += elapseSeconds;
                            if (m_CurClipLength > m_ClipMaxLength)
                            {

                                m_CurClipLength = 0;
                                if (m_PennyAnimator.GetInteger("state") != 0)
                                    m_PennyAnimator.SetInteger("state", 0);
                            }
                        }
                        Debug.Log(m_CurTime);
                        if ((int)m_CurTime==11||(int)m_CurTime==5)
                        {
                            GameEntry.Sound.PlayUISound((int)UISoundId.HeartBeat);
                        }

                        if (m_CurTime < 0)
                        {
                            m_CurTime = 0;
                            GameEntry.Sound.PlayUISound((int)UISoundId.TimeOver);
                            EnterFailed();
                        }
                    }
                    break;
                case Produceing.Success:
                    {
                        //GameEntry.Sound.StopSound(TopicTouchBase.s_SoundSerialId);
                        //GameEntry.Sound.PlayUISound((int)UISoundId.Success);
                        Enter_WaitNext();
                        /////答题成功有分数和星星声音
                        //m_Istingting = true;
                        //m_IsUpScore = true;
                    }
                    break;
                case Produceing.Failed:
                    {
                        //GameEntry.Sound.StopSound(TopicTouchBase.s_SoundSerialId);
                        //GameEntry.Sound.PlayUISound((int)UISoundId.Failed);
                        Enter_WaitNext();
                        /////答题失败没有分数和星星声音
                        //m_Istingting = false;
                        //m_IsUpScore = false;
                    }
                    break;
                case Produceing.WaitNext:
                    {
                        m_CurTime += elapseSeconds;
                        if (m_CurTime>m_ClipMaxLength)
                        {
                            if (m_PennyAnimator.GetInteger("state") != 0)
                                m_PennyAnimator.SetInteger("state", 0);
                        }
                        //完成游戏之后的操作
                        //完成答题涨星星的叮叮声
                        StartCoroutine(IEnumeratorNext());
                        StartCoroutine(IEnumeratorUpScore());
                        
                        if (m_CurTime > 5f)
                        {
                            ResetData();
                            m_TopicCale.Small();
                            Enter_CountDown();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnLidarHitEvent(GameObject go, Vector3 vec)
        {
            if (m_ProduceingState != Produceing.Playing) return;
            TouchProp(go);
            TouchDrawer(go);
        }

        private void TouchProp(GameObject go)
        {
            if (go == null) return;
            if (m_IsChooseSuccessCondition) return;
            for (int i = 0; i < m_DrawerProps.Length; i++)
            {
                if (m_DrawerProps[i].gameObject == go)
                {
                    m_SelectProps = m_DrawerProps[i];
                    if (m_IsChooseSuccessCondition = m_DrawerProps[i].Select(m_CurPropType))
                    {
                        for (int j = 0; j < m_DrawerProps.Length; j++)
                        {
                            if (m_DrawerProps[j].gameObject != go)
                            {
                                m_DrawerProps[j].Gary();
                            }
                        }
                    }
                    else
                    {
                        
                        m_PennyAnimator.SetInteger("state", 6);
                    }
                    break;
                }
            }
        }

        private void TouchDrawer(GameObject go)
        {
            if (go == null) return;
            if (!m_IsChooseSuccessCondition) return;
            if (m_IsChooseSuccessTarget) return;

            if (m_DifficultyType == DifficultyType.Easy)
            {
                for (int i = 0; i < m_DrawerTouch_L.Length; i++)
                {
                    if (m_DrawerTouch_L[i].TouchObj == go)
                    {
                        m_SelectDrawer = m_DrawerTouch_L[i];
                        if (m_IsChooseSuccessTarget = m_DrawerTouch_L[i].Select(m_CurLeftRightDir, m_SuccessID, (x) => { PropMove(x); }))
                        {
                            for (int j = 0; j < m_DrawerTouch_L.Length; j++)
                            {
                                if (m_DrawerTouch_L[j].gameObject != go)
                                {
                                    m_DrawerTouch_L[j].Cancel();
                                    //m_DrawerTouch_R[j].Cancel();
                                }
                            }
                        }
                        break;
                    }
                }
            }
            else if (m_DifficultyType == DifficultyType.Trouble)
            {
                for (int i = 0; i < m_DrawerTouch_L_2.Length; i++)
                {
                    if (m_DrawerTouch_L_2[i].TouchObj == go)
                    {
                        m_SelectDrawer = m_DrawerTouch_L_2[i];
                        if (m_IsChooseSuccessTarget = m_DrawerTouch_L_2[i].Select(m_CurLeftRightDir, m_SuccessID, (x) => { PropMove(x); }))
                        {
                            for (int j = 0; j < m_DrawerTouch_L_2.Length; j++)
                            {
                                if (m_DrawerTouch_L_2[j].gameObject != go)
                                {
                                    m_DrawerTouch_L_2[j].Cancel();
                                    m_DrawerTouch_R[j].Cancel();
                                }
                            }
                        }
                        break;
                    }
                }
                for (int i = 0; i < m_DrawerTouch_R.Length; i++)
                {
                    if (m_DrawerTouch_R[i].TouchObj == go)
                    {
                        m_SelectDrawer = m_DrawerTouch_R[i];
                        if (m_IsChooseSuccessTarget = m_DrawerTouch_R[i].Select(m_CurLeftRightDir, m_SuccessID, (x) => { PropMove(x); }))
                        {
                            for (int j = 0; j < m_DrawerTouch_R.Length; j++)
                            {
                                if (m_DrawerTouch_R[j].gameObject != go)
                                {
                                    m_DrawerTouch_L[j].Cancel();
                                    m_DrawerTouch_R[j].Cancel();
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void PropMove(Drawer drawer, bool isGuide = false)
        {
            if (m_SelectProps == null) return;
            float x = UnityEngine.Random.Range(-1f, 1f);
            Vector3 newPos = drawer.transform.position + new Vector3(x, 0, 0);

            Vector3[] path = new Vector3[3]
            {
                m_SelectProps.transform.position,
                m_SelectProps.transform.position - new Vector3(3,-3,0),
                newPos
            };
            Tween tween1 = m_SelectProps.transform.DOPath(path, 0.5f, PathType.CatmullRom).SetEase(Ease.InCubic);
            GameEntry.Sound.PlayUISound((int)UISoundId.biu);
            tween1.OnComplete(() =>
            {
                m_SelectProps.ResetProp();
                drawer.Close();
                DrawerProp prop = CloneProp(drawer.Parent, newPos);
                prop.Clone();
                if (!isGuide)
                { 
                    Enter_Success();
                    
                }
            });
        }

        private DrawerProp CloneProp(Transform trs, Vector3 pos)
        {
            DrawerProp item = Instantiate(m_SelectProps);
            Transform transform = item.GetComponent<Transform>();
            transform.SetParent(trs);
            transform.position = pos;
            transform.localScale = Vector3.one * 0.8f;
            return item;
        }

        protected override void ResetCallBack()
        {
            InitAnswer();
            foreach (var item in m_DrawerTouch_L)
            {
                item.ClearParentNode();
            }
            foreach (var item in m_DrawerTouch_L_2)
            {
                item.ClearParentNode();
            }
            foreach (var item in m_DrawerTouch_R)
            {
                item.ClearParentNode();
            }
            for (int i = 0; i < m_DrawerProps.Length; i++)
            {
                m_DrawerProps[i].StopAutoScale();
                m_DrawerProps[i].ClearScale();
                m_DrawerProps[i].ClearColor();
            }
            base.ResetCallBack();
        }

        protected override void ChangeCallBack()
        {
            base.ChangeCallBack();
            InitAnswer();
            foreach (var item in m_DrawerTouch_L)
            {
                item.ClearParentNode();
            }
            foreach (var item in m_DrawerTouch_L_2)
            {
                item.ClearParentNode();
            }
            foreach (var item in m_DrawerTouch_R)
            {
                item.ClearParentNode();
            }

            for (int i = 0; i < m_DrawerProps.Length; i++)
            {
                m_DrawerProps[i].StopAutoScale();
                m_DrawerProps[i].ClearScale();
                m_DrawerProps[i].ClearColor();
            }
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        protected override void ResetData()
        {
            base.ResetData();
            m_SelectProps = null;
            m_SelectDrawer = null;
        }

        protected override void InitAnswer()
        {
            m_NewProps = new PropType[] {
                PropType.Toys, PropType.Cothes, PropType.Book, PropType.Snacks,
                PropType.Toys, PropType.Cothes, PropType.Book, PropType.Snacks,
                (PropType)Utility.Random.GetRandom((int)PropType.Toys, (int)PropType.Snacks),};
            m_NewProps = GetRandomArray(m_NewProps);

            for (int i = 0; i < m_DrawerProps.Length; i++)
            {
                Sprite[] sprites = GetPropSpriteByType(m_NewProps[i]);
                //int id = Utility.Random.GetRandom(0, sprites.Length);
                Sprite iconSprite = PropWeightRemoval(sprites);
                m_DrawerProps[i].SetSprite(m_NewProps[i], iconSprite, 0);
            }

            m_IsFristMove = true;
        }

        protected override void InitBackGround(DifficultyType type)
        {
            base.InitBackGround(type);
            Vector3[] ban_1 = new Vector3[] { new Vector3(-619, -50, 0), new Vector3(-619, -50, 0), new Vector3(-154, -160, 0), };
            Vector3[] ban_2 = new Vector3[] { new Vector3(-751, -50, 0), new Vector3(-265, -50, 0), new Vector3(-9, -160, 0), };
            if (type == DifficultyType.Easy)
            {
                m_LeftBan.gameObject.SetActive(true);
                m_LeftBan_2.gameObject.SetActive(false);
                m_RightBan.gameObject.SetActive(false);
                //m_LeftBan.transform.localPosition = ban_1[0];
                //m_RightBan.transform.localPosition = ban_1[1];

                m_LeftBan.transform.localPosition = ban_1[0] + Vector3.up * 1080;
                m_PropTrs.transform.localPosition = ban_1[2];

                m_BoomEffect.Rewind(() =>
                {
                    m_LeftBan.transform.DOLocalMoveY(m_LeftBan.transform.localPosition.y - 1080, 0.5f);
                });
                Enter_PanBai();
            }
            else if (type == DifficultyType.Trouble)
            {
                m_LeftBan.gameObject.SetActive(false);
                m_LeftBan_2.gameObject.SetActive(true);
                m_RightBan.gameObject.SetActive(true);

                m_LeftBan_2.transform.localPosition = ban_2[0] + Vector3.up * 1080;
                m_RightBan.transform.localPosition = ban_2[1] + Vector3.up * 1080;
                m_PropTrs.transform.localPosition = ban_2[2];

                m_BoomEffect.Rewind(() =>
                {
                    m_LeftBan_2.transform.DOLocalMoveY(m_LeftBan_2.transform.localPosition.y - 1080, 0.5f);
                    m_RightBan.transform.DOLocalMoveY(m_RightBan.transform.localPosition.y - 1080, 0.5f);
                });
                Enter_PennySay_Trouble();
            }
        }
        /// <summary>
        /// 初始化皮尼
        /// </summary>
        private void InitPenny()
        {
            m_PennyAnimator.transform.localPosition = m_InitPos;
            m_PennyAnimator.SetInteger("state", 1);
            m_PennyAnimator.transform.DOLocalMoveX(m_PennyAnimator.transform.localPosition.x - 430, 2f).OnComplete(() =>
            {
                m_PennyAnimator.SetInteger("state", 5);
                //m_PennyAnimator.transform.DOLocalMoveX(m_PennyAnimator.transform.localPosition.x, 1).OnComplete(() =>
                //{
                //    m_PennyAnimator.SetInteger("state", 6);
                //});
            });

        }

        protected override void Enter_PanBai()
        {
            base.Enter_PanBai();
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_PanbaiSoundId);
            s_PanbaiSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_pangbai1, ref m_ClipMaxLength);
        }

        protected override void Enter_Teaching()
        {
            base.Enter_Teaching();
            for (int i = 0; i < m_DrawerProps.Length; i++)
            {
                m_DrawerProps[i].Scale();
                if (m_DrawerProps[i].PropType == PropType.Snacks && m_SelectProps == null)
                {
                    m_SelectProps = m_DrawerProps[i];
                }
            }
            Drawer selectDrawer = null;
            for (int i = 0; i < m_DrawerTouch_L.Length; i++)
            {
                if (m_DrawerTouch_L[i].Id == 5)
                {
                    selectDrawer = m_DrawerTouch_L[i];
                    break;
                }
            }
            m_Guide.StartGuide(m_SelectProps.transform, selectDrawer.transform,
                () =>
                {
                    m_SelectProps.Select(PropType.Snacks);
                    for (int j = 0; j < m_DrawerProps.Length; j++)
                    {
                        if (m_DrawerProps[j].gameObject != m_SelectProps)
                        {
                            m_DrawerProps[j].Gary();
                        }
                    }
                },
                () =>
                {
                    selectDrawer.Select(DirType.Left, 5, (x) => { PropMove(x, true); });
                },
                m_PennyAnimator
            );
        }

        protected override void Enter_PennySay()
        {
            base.Enter_PennySay();
            m_PennyAnimator.SetInteger("state", 6);
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_PennySaySoundId);
            s_PennySaySoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_pangbai2, ref m_ClipMaxLength);
        }

        protected override void Enter_PennySay_Trouble()
        {
            base.Enter_PennySay();
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_PennySaySoundId);
            s_PennySaySoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_pangbai3, ref m_ClipMaxLength);
        }

        protected override void Enter_CountDown()
        {
            base.Enter_CountDown();
            m_CountDown.StartCountDown(null);
        }

        /// <summary>
        /// 进入刷头像
        /// </summary>
        protected override void Enter_BrushTopic()
        {
            base.Enter_BrushTopic();
            m_CurPropType = (PropType)Enum.Parse(typeof(PropType), SelectTopicSplit[0]);
            m_CurLeftRightDir = (DirType)Enum.Parse(typeof(DirType), SelectTopicSplit[1]);
            m_CurUpDownDir = (DirType)Enum.Parse(typeof(DirType), SelectTopicSplit[2]);
            if (DirType.Up == m_CurUpDownDir)
            {
                m_SuccessID = int.Parse(SelectTopicSplit[3]);
            }
            else if (DirType.Down == m_CurUpDownDir)
            {
                m_SuccessID = 5 - int.Parse(SelectTopicSplit[3]) + 1;
            }
            m_CurSoundId = int.Parse(SelectTopicSplit[4]);
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_TopicSoundId);
            s_TopicSoundId = (int)GameEntry.Sound.PlaySoundAndLength(m_CurSoundId, ref m_ClipMaxLength);
            //m_PennyAnimator.SetInteger("state", 6);
            // 出道具
            int NoShowNum = 0;
            for (int i = 0; i < m_DrawerProps.Length; i++)
            {
                if (!m_DrawerProps[i].IsShow)
                {
                    NoShowNum++;
                }
            }
            if (NoShowNum >= 3)
            {
                MaxThanThree(m_DrawerProps);
                for (int i = 0; i < m_DrawerProps.Length; i++)
                {
                    m_DrawerProps[i].ClearColor();
                    if (!m_DrawerProps[i].IsShow)
                    {
                        Sprite[] sprites = GetPropSpriteByType(m_NewProps[i]);
                        //int id = Utility.Random.GetRandom(0, sprites.Length);
                        Sprite iconSprite = PropWeightRemoval(sprites);
                        m_DrawerProps[i].SetSprite(m_NewProps[i], iconSprite, 0);
                        m_DrawerProps[i].Scale();
                    }
                }
            }
            else
            {
                if (m_IsFristMove)
                {
                    m_IsFristMove = false;
                    for (int i = 0; i < m_DrawerProps.Length; i++)
                    {
                        if (m_DrawerProps[i].IsShow)
                            m_DrawerProps[i].Scale();
                    }
                }
            }
            m_IsTips = false;
        }

        protected override void Enter_WaitNext()
        {
            base.Enter_WaitNext();
            m_PennyAnimator.SetInteger("state", 6);
            if (TimeOut && m_SelectProps)
            {
                m_SelectProps.OutTime();
            }
            for (int i = 0; i < m_DrawerProps.Length; i++)
            {
                m_DrawerProps[i].StopAutoScale();
                m_DrawerProps[i].ClearScale();
                m_DrawerProps[i].ClearColor();
            }
            for (int i = 0; i < m_DrawerTouch_L.Length; i++)
            {
                m_DrawerTouch_L[i].Cancel();
                m_DrawerTouch_L_2[i].Cancel();
                m_DrawerTouch_R[i].Cancel();
            }
        }

        private float m_ShakeTime = 0;
        private float m_ShakeMaxTime = 1f;
        
       
        /// <summary>
        /// 不定时的随机跳跃
        /// </summary>
        private void RandomJump()
        {
            if (m_IsChooseSuccessCondition) return;
            if (m_IsTips) return;
            m_ShakeTime++;
            if (m_ShakeTime>=m_ShakeMaxTime)
            {
                m_ShakeTime = 0;
                int randomid = Utility.Random.GetRandom(m_DrawerProps.Length);
                if (m_DrawerProps[randomid].IsShow)
                {
                    m_DrawerProps[randomid].StartJump();
                }
             
            }
            
        }

        //时间还剩1/4时，提示正确答案
        private void Tips()
        {
            if (m_IsChooseSuccessCondition) return;
            if (!m_IsTips && m_CurTime < AnswerTime / 4f)
            {
                m_IsTips = true;
                GameEntry.Sound.PlayUISound((int)UISoundId.Tips);
                for (int i = 0; i < m_DrawerProps.Length; i++)
                {
                    if (m_DrawerProps[i].PropType == m_CurPropType)
                    {
                        m_DrawerProps[i].StartAutoScale();
                    }
                }
            }
        }

        ///// <summary>
        ///// 随机出题,满四次重新来过
        ///// </summary>
        //protected override string RandomTopic()
        //{
        //    //// 随机一个包含显示道具的题目
        //    //string[] Ti = (m_DifficultyType == DifficultyType.Easy) ? EasyTopic : TroubleTopic;
        //    //List<PropType> list = new List<PropType>();
        //    //for (int i = 0; i < m_DrawerProps.Length; i++)
        //    //{
        //    //    if (m_DrawerProps[i].IsShow)
        //    //    {
        //    //        list.Add(m_DrawerProps[i].PropType);
        //    //    }
        //    //}
        //    //int index = Utility.Random.GetRandom(0, list.Count);
        //    //for (int i = 0; i < Ti.Length; i++)
        //    //{
        //    //    string[] sub = Ti[i].Split(';');
        //    //    if ((PropType)Enum.Parse(typeof(PropType), sub[0]) == list[index])
        //    //    {
        //    //        return Ti[i];
        //    //    }
        //    //}
        //    //return "";
        //}

        private PropType[] GetRandomArray(PropType[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                PropType temp = array[i];
                int randomIndex = Utility.Random.GetRandom(0, array.Length);
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }
            return array;
        }

        private Sprite[] GetPropSpriteByType(PropType prop)
        {
            switch (prop)
            {
                case PropType.Toys:
                    return m_ToysSprites;
                case PropType.Cothes:
                    return m_CothesSprites;
                case PropType.Book:
                    return m_BookSprites;
                case PropType.Snacks:
                    return m_SnacksSprites;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 道具图片去重
        /// </summary>
        /// <param name="sprites"></param>
        /// <param name="sprite"></param>
        /// <returns></returns>
        private Sprite PropWeightRemoval(Sprite[] sprites)
        {
            List<Sprite> same = new List<Sprite>();
            for (int i = 0; i < m_DrawerProps.Length; i++)
            {
                for (int j = 0; j < sprites.Length; j++)
                {
                    if (m_DrawerProps[i].Icon.sprite.name.Contains(sprites[j].name))
                    {
                        same.Add(sprites[j]);
                    }
                }
            }

            List<Sprite> no_same = new List<Sprite>();
            for (int i = 0; i < sprites.Length; i++)
            {
                Sprite sp = same.Find((x) => { return x.name.Contains(sprites[i].name); });
                if (sp == null)
                {
                    no_same.Add(sprites[i]);
                }
            }
            if (no_same.Count <= 0)
            {
                Log.Error("Function 'PropWeightRemoval' is invalid");
                return sprites[0];
            }

            int id = Utility.Random.GetRandom(0, no_same.Count);
            return no_same[id];
        }

        private PropType[] temppp = new PropType[] { PropType.Toys, PropType.Cothes, PropType.Book, PropType.Snacks };

        private void MaxThanThree(DrawerProp[] prop)
        {
            PropType[] newTemp = new PropType[prop.Length];
            for (int i = 0; i < newTemp.Length; i++)
            {
                newTemp[i] = PropType.None;
            }
            for (int i = 0; i < newTemp.Length; i++)
            {
                for (int j = 0; j < prop.Length; j++)
                {
                    if (prop[j].IsShow)
                    {
                        newTemp[i] = prop[j].PropType;
                    }
                }
            }
            FixOneData(newTemp);
        }

        private void FixOneData(PropType[] propTypes)
        {
            PropType temp = temppp[Utility.Random.GetRandom(0, temppp.Length)];

            for (int i = 0; i < propTypes.Length; i++)
            {
                if (propTypes[i] == temp)
                {
                    FixOneData(propTypes);
                    return;
                }
                else
                {
                    propTypes[i] = temp;
                }
            }

            for (int i = 0; i < propTypes.Length; i++)
            {
                if (propTypes[i] == PropType.None)
                {
                    temp = temppp[Utility.Random.GetRandom(0, temppp.Length)];
                    propTypes[i] = temp;
                }
            }
        }

    }
}
