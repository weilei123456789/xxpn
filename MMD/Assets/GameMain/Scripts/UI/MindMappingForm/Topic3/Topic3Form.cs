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
    public class Topic3Form : TopicBase
    {
        public override string[] EasyTopic
        {
            get
            {
                string[] _ti = new string[]
                {
                    string.Format("{0};{1};{2};{3}", ColorType.Green, "green_easy_L", "farm_easy_R",     (int)SoundId.s3_easy_1),
                    string.Format("{0};{1};{2};{3}", ColorType.Red,   "red_easy_L", "farm_easy_R",       (int)SoundId.s3_easy_2),
                    string.Format("{0};{1};{2};{3}", ColorType.Purple,"purple_easy_L", "water_easy_R",  (int)SoundId.s3_easy_3),
                    string.Format("{0};{1};{2};{3}", ColorType.Black, "black_easy_L", "desert_easy_R",   (int)SoundId.s3_easy_4),
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
                    string.Format("{0};{1};{2};{3}", ColorType.Brown, "brown_trouble_L", "desert_trouble_R",        (int)SoundId.s3_diff_1),
                    string.Format("{0};{1};{2};{3}", ColorType.Blue,   "blue_trouble_L", "water_trouble_R",         (int)SoundId.s3_diff_2),
                    string.Format("{0};{1};{2};{3}", ColorType.Red, "red_trouble_L", "farm_trouble_R",              (int)SoundId.s3_diff_3),
                    string.Format("{0};{1};{2};{3}", ColorType.Orange,  "orange_trouble_L", "desert_trouble_R",     (int)SoundId.s3_diff_4),
                    string.Format("{0};{1};{2};{3}", ColorType.Purple, "purple_trouble_L", "water_trouble_R",       (int)SoundId.s3_diff_5),
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
        private Shovel m_Shovel = null;
        [SerializeField]
        private Transform m_ShovelParent = null;
        [SerializeField]
        private Sprite[] m_MapSprite = null;
        [SerializeField]
        private Transform m_EasyTransform = null;
        [SerializeField]
        private Image m_EasyMapLeft = null;
        [SerializeField]
        private Image m_EasyMapRight = null;
        [SerializeField]
        private Transform m_TroubleTransform = null;
        [SerializeField]
        private Image m_TroubleMapLeft = null;
        [SerializeField]
        private Image m_TroubleMapRight = null;

        [SerializeField]
        private TreasureMap[] m_EasyTreasureMap = null;
        [SerializeField]
        private Graphical[] m_EasyGraphical = null;

        [SerializeField]
        private TreasureMap[] m_TroubleTreasureMap = null;
        [SerializeField]
        private Graphical[] m_TroubleGraphical = null;

        [SerializeField]
        private Sprite[] m_TriangleSprite = null;
        [SerializeField]
        private Sprite[] m_DiamondSprite = null;
        [SerializeField]
        private Sprite[] m_CrossSprite = null;
        [SerializeField]
        private Sprite[] m_ForkSprite = null;
        [SerializeField]
        private Sprite[] m_SquareSprite = null;
        [SerializeField]
        private Sprite[] m_CircularSprite = null;

        private GraphicalType[] m_TypeName = new GraphicalType[] { GraphicalType.Triangle, GraphicalType.Diamond, GraphicalType.Cross, GraphicalType.Fork, GraphicalType.Square, GraphicalType.Circular, };

        private List<GraphicalType> m_CurTopicType = new List<GraphicalType>();

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.VideoPlayer.PlayLoadMovice("XXPN_EP01_sc002_light_BG_H");
            GameEntry.Sound.PlayMusic((int)MusicId.topic_3);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        protected override void OnLidarHitEvent(GameObject go, Vector3 vec)
        {
            if (m_ProduceingState != Produceing.Playing) return;
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
                        CheckSuccess();
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
            s_PanbaiSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s3_panbai_1, ref m_ClipMaxLength);
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
            s_PennySaySoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s3_panbai_2, ref m_ClipMaxLength);
        }

        protected override void Enter_PennySay_Trouble()
        {
            base.Enter_PennySay();
            // 蓝队也加入比赛，快看~他们来了
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_PennySaySoundId);
            s_PennySaySoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s3_panbai_3, ref m_ClipMaxLength);
        }

        protected override void Enter_CountDown()
        {
            base.Enter_CountDown();
            m_CountDown.StartCountDown(null);
        }

        protected override void Enter_BrushTopic()
        {
            base.Enter_BrushTopic();
            m_CurTopicType.Clear();
            m_CurColor = (ColorType)Enum.Parse(typeof(ColorType), SelectTopicSplit[0]);
            //m_GraphicalType = (GraphicalType)Enum.Parse(typeof(GraphicalType), SelectTopicSplit[1]);
            if (m_DifficultyType == DifficultyType.Easy)
            {
                m_EasyMapLeft.sprite = GetBackGroundSprite(SelectTopicSplit[1]);
                m_EasyMapRight.sprite = GetBackGroundSprite(SelectTopicSplit[2]);
                m_EasyTransform.localScale = Vector3.one;
            }
            else
            {
                m_TroubleMapLeft.sprite = GetBackGroundSprite(SelectTopicSplit[1]);
                m_TroubleMapRight.sprite = GetBackGroundSprite(SelectTopicSplit[2]);
                m_TroubleTransform.localScale = Vector3.one;
            }
            m_CurSoundId = int.Parse(SelectTopicSplit[3]);
            m_SuccessID = -1;
            m_CurClipLength = 0;
            GameEntry.Sound.StopSound(s_TopicSoundId);
            s_TopicSoundId = (int)GameEntry.Sound.PlaySoundAndLength(m_CurSoundId, ref m_ClipMaxLength);
            // 出题
            if (m_DifficultyType == DifficultyType.Easy)
            {
                for (int i = 0; i < 16; i += 2)
                {
                    int index = (i + Utility.Random.GetRandom(0, 2));
                    m_GraphicalType = m_TypeName[Utility.Random.GetRandom(0, m_TypeName.Length)];
                    m_EasyTreasureMap[index].Topic(GetGraphicalSprite_Normal(m_GraphicalType), m_CurColor, m_GraphicalType);
                }
                int[] realIndex = Correct(2, m_EasyTreasureMap);
                for (int i = 0; i < realIndex.Length; i++)
                {
                    int index = realIndex[i];
                    m_EasyTreasureMap[index].Correct(GetGraphicalSprite_RimLight(m_EasyTreasureMap[index].GraphicalType), m_EasyTreasureMap[index].GraphicalType);
                    m_EasyGraphical[index].Topic(m_EasyTreasureMap[index].ColorType, m_EasyTreasureMap[index].GraphicalType, this);
                    m_CurTopicType.Add(m_EasyTreasureMap[index].GraphicalType);
                }
            }
            else if (m_DifficultyType == DifficultyType.Trouble)
            {
                int[] array = new int[m_TroubleTreasureMap.Length];
                for (int i = 0; i < array.Length; i++) { array[i] = i; }
                int[] newArrayIndexs = GetRandomArray(array);
                for (int i = 0; i < newArrayIndexs.Length; i++)
                {
                    if (i >= 9) break;
                    int index = newArrayIndexs[i];
                    m_GraphicalType = m_TypeName[Utility.Random.GetRandom(0, m_TypeName.Length)];
                    m_TroubleTreasureMap[index].Topic(GetGraphicalSprite_Normal(m_GraphicalType), m_CurColor, m_GraphicalType);
                }
                int[] realIndex = Correct(3, m_TroubleTreasureMap);
                for (int i = 0; i < realIndex.Length; i++)
                {
                    int index = realIndex[i];
                    m_TroubleTreasureMap[index].Correct(GetGraphicalSprite_RimLight(m_TroubleTreasureMap[index].GraphicalType), m_TroubleTreasureMap[index].GraphicalType);
                    m_TroubleGraphical[index].Topic(m_TroubleTreasureMap[index].ColorType, m_TroubleTreasureMap[index].GraphicalType, this);
                    m_CurTopicType.Add(m_TroubleTreasureMap[index].GraphicalType);
                }
            }
        }

        private int[] Correct(int count, TreasureMap[] resource)
        {
            List<int> resourceIndex = new List<int>();
            for (int i = 0; i < resource.Length; i++)
            {
                if (resource[i].GraphicalType != GraphicalType.None)
                {
                    resourceIndex.Add(i);
                }
            }
            int[] tempNew = GetRandomArray(resourceIndex.ToArray());
            int[] newArray = new int[count];
            for (int i = 0; i < count; i++)
            {
                newArray[i] = tempNew[i];
            }
            return newArray;
        }

        private int[] GetRandomArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int temp = array[i];
                int randomIndex = Utility.Random.GetRandom(0, array.Length);
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }
            return array;
        }

        protected override void Enter_WaitNext()
        {
            base.Enter_WaitNext();
        }

        protected override void InitBackGround(DifficultyType type)
        {
            base.InitBackGround(type);
            if (type == DifficultyType.Easy)
            {
                m_EasyTransform.gameObject.SetActive(true);
                m_EasyTransform.localScale = Vector3.zero;
                m_TroubleTransform.gameObject.SetActive(false);
                m_TroubleTransform.localScale = Vector3.zero;
                Enter_PanBai();
            }
            else if (type == DifficultyType.Trouble)
            {
                m_EasyTransform.gameObject.SetActive(false);
                m_EasyTransform.localScale = Vector3.zero;
                m_TroubleTransform.gameObject.SetActive(true);
                m_TroubleTransform.localScale = Vector3.zero;
                Enter_PennySay_Trouble();
            }
        }

        protected override void InitAnswer()
        {
            //for (int i = 0; i < m_EasyTreasureMap.Length; i++)
            //{
            //    m_EasyTreasureMap[i].Clear();
            //}
            //for (int i = 0; i < m_EasyGraphical.Length; i++)
            //{
            //    m_EasyGraphical[i].Clear();
            //}
        }

        /// <summary>
        /// open时候调用一次
        /// </summary>
        protected override void ResetData()
        {
            base.ResetData();
            //m_SelectTreasureMap = null;
            for (int i = 0; i < m_EasyTreasureMap.Length; i++)
            {
                m_EasyTreasureMap[i].Clear();
            }
            for (int i = 0; i < m_EasyGraphical.Length; i++)
            {
                m_EasyGraphical[i].Clear();
            }
            for (int i = 0; i < m_TroubleTreasureMap.Length; i++)
            {
                m_TroubleTreasureMap[i].Clear();
            }
            for (int i = 0; i < m_TroubleGraphical.Length; i++)
            {
                m_TroubleGraphical[i].Clear();
            }
            m_CurTopicType.Clear();

            m_EasyTransform.localScale = Vector3.zero;
            m_TroubleTransform.localScale = Vector3.zero;
            ClearShovelTrs();
        }

        private void TouchTarget(GameObject go)
        {
            if (go == null) return;
            if (go.name.Contains("TreasureMap")) return;
            //if (!m_IsChooseSuccessCondition) return;
            //if (m_IsChooseSuccessTarget) return;

            if (m_DifficultyType == DifficultyType.Easy)
            {
                for (int i = 0; i < m_EasyGraphical.Length; i++)
                {
                    m_EasyGraphical[i].Select(go, null);
                }
            }
            else if (m_DifficultyType == DifficultyType.Trouble)
            {
                for (int i = 0; i < m_TroubleGraphical.Length; i++)
                {
                    m_TroubleGraphical[i].Select(go, null);
                }
            }
        }


        public void CloneProp(Vector3 pos)
        {
            //初始化铲子
            Shovel item = Instantiate(m_Shovel);
            Transform transform = item.GetComponent<Transform>();
            transform.SetParent(m_ShovelParent);
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
            item.ShovelShake(pos);
        }

        private void CheckSuccess()
        {
            bool isSuccess = true;
            if (m_DifficultyType == DifficultyType.Easy)
            {
                for (int i = 0; i < m_EasyGraphical.Length; i++)
                {
                    foreach (var item in m_CurTopicType)
                    {
                        if (m_EasyGraphical[i].GraphicalType == item)
                        {
                            if (!m_EasyGraphical[i].IsComplete)
                            {
                                isSuccess = false;
                                break;
                            }
                        }
                    }
                }
            }
            else if (m_DifficultyType == DifficultyType.Trouble)
            {
                for (int i = 0; i < m_TroubleGraphical.Length; i++)
                {
                    foreach (var item in m_CurTopicType)
                    {
                        if (m_TroubleGraphical[i].GraphicalType == item)
                        {
                            if (!m_TroubleGraphical[i].IsComplete)
                            {
                                isSuccess = false;
                                break;
                            }
                        }
                    }
                }
            }

            if (isSuccess)
            {
               
                Enter_Success();
            }
        }
       

        public Sprite GetGraphicalSprite_Normal(GraphicalType type)
        {
            switch (type)
            {
                case GraphicalType.Triangle:
                    return m_TriangleSprite[0];
                case GraphicalType.Diamond:
                    return m_DiamondSprite[0];
                case GraphicalType.Cross:
                    return m_CrossSprite[0];
                case GraphicalType.Fork:
                    return m_ForkSprite[0];
                case GraphicalType.Square:
                    return m_SquareSprite[0];
                case GraphicalType.Circular:
                    return m_CircularSprite[0];
                default:
                    return null;
            }
        }
        public Sprite GetGraphicalSprite_RimLight(GraphicalType type)
        {
            switch (type)
            {
                case GraphicalType.Triangle:
                    return m_TriangleSprite[1];
                case GraphicalType.Diamond:
                    return m_DiamondSprite[1];
                case GraphicalType.Cross:
                    return m_CrossSprite[1];
                case GraphicalType.Fork:
                    return m_ForkSprite[1];
                case GraphicalType.Square:
                    return m_SquareSprite[1];
                case GraphicalType.Circular:
                    return m_CircularSprite[1];
                default:
                    return null;
            }
        }

        public Sprite[] GetSplitGraphicalSprite(GraphicalType type)
        {
            switch (type)
            {
                case GraphicalType.Triangle:
                    return CopyGraphicalArray(m_TriangleSprite, 2);
                case GraphicalType.Diamond:
                    return CopyGraphicalArray(m_DiamondSprite, 2);
                case GraphicalType.Cross:
                    return CopyGraphicalArray(m_CrossSprite, 2);
                case GraphicalType.Fork:
                    return CopyGraphicalArray(m_ForkSprite, 2);
                case GraphicalType.Square:
                    return CopyGraphicalArray(m_SquareSprite, 2);
                case GraphicalType.Circular:
                    return CopyGraphicalArray(m_CircularSprite, 2);
            }
            return null;
        }

        private Sprite[] CopyGraphicalArray(Sprite[] sprites, int startIndex)
        {
            if ((sprites.Length - startIndex) <= 0)
            {
                return null;
            }
            Sprite[] spriteTemp = new Sprite[sprites.Length - startIndex];
            for (int i = 0; i < spriteTemp.Length; i++)
            {
                spriteTemp[i] = sprites[i + startIndex];
            }
            return spriteTemp;
        }

        private Sprite GetBackGroundSprite(string name)
        {
            for (int i = 0; i < m_MapSprite.Length; i++)
            {
                if (m_MapSprite[i].name.Equals(name))
                {
                    return m_MapSprite[i];
                }
            }
            return null;
        }

        private void ClearShovelTrs()
        {
            for (int i = m_ShovelParent.childCount-1; i >= 0; i--)
            {
                Destroy(m_ShovelParent.GetChild(i).gameObject);
            }
        }
    }
}