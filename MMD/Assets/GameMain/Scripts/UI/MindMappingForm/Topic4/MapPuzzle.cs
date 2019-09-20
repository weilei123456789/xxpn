using DG.Tweening;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penny
{
    public class MapPuzzle : TopicTouchBase
    {
        protected override int TouchSuccessUISoundId
        {
            get { return (int)UISoundId.qipao_s; }
        }

        protected override int TouchFailedUISoundId
        {
            get { return (int)UISoundId.qipao_f; }
        }

        public override GameObject TouchObj
        {
            get { return gameObject; }
        }

        [Header("------------------------------")]
        [SerializeField]
        protected DifficultyType m_DifficultyType = DifficultyType.None;
        [SerializeField]
        private GameObject[] m_TouchObjs = null;
        [SerializeField]
        private Image[] m_EdgeImages = null;
        [SerializeField]
        private Image m_CompleteImage = null;
        [SerializeField]
        private GameObject[] m_PropTouchObjs = null;
        [SerializeField]
        private GameObject[] m_PropObjs = null;

        private GameFrameworkAction OnComplatre = null;
        private MapPuzzleData[] m_MapPuzzleDatas = null;
        private bool[] m_PropFlags = null;

        private bool m_IsComplete = false;
        private bool m_IsPropComplete = true;

        //public bool IsComplete
        //{
        //    get { return m_IsComplete; }
        //}

        protected override void Awake()
        {
            InitTopic();
        }

        protected override void Start()
        {
            Clear();
        }

        public override bool Select(GameObject go, GameFrameworkAction finish)
        {
            if (!m_IsComplete)
            {
                for (int i = 0; i < m_MapPuzzleDatas.Length; i++)
                {
                    if (m_MapPuzzleDatas[i].Select(go))
                    {
                        SelectSuccess();
                    }
                }
                m_IsComplete = IsAllComplete();
                if (m_IsComplete)
                {
                    ComplateFirst();
                }
            }
            else
            {
                if (m_IsPropComplete) return true;

                for (int i = 0; i < m_PropTouchObjs.Length; i++)
                {
                    if (m_PropTouchObjs[i] == go && !m_PropFlags[i])
                    {
                        m_PropFlags[i] = true;
                        SelectSuccess();
                        Scale(m_PropObjs[i]);

                        m_IsPropComplete = IsAllPropComplete();
                        if (m_IsPropComplete)
                        {
                            if (OnComplatre != null)
                                OnComplatre();
                        }
                    }
                }
            }
            return false;
        }

        public override void Clear()
        {
            for (int i = 0; i < m_MapPuzzleDatas.Length; i++)
            {
                m_MapPuzzleDatas[i].Clear();
            }
            for (int i = 0; i < m_PropObjs.Length; i++)
            {
                m_PropObjs[i].transform.localScale = Vector3.zero;
            }
            for (int i = 0; i < m_EdgeImages.Length; i++)
            {
                m_EdgeImages[i].enabled = false;
            }
            for (int i = 0; i < m_PropFlags.Length; i++)
            {
                m_PropFlags[i] = false;
            }
            m_CompleteImage.enabled = false;
            OnComplatre = null;
            m_IsComplete = false;
            m_IsPropComplete = true;
            gameObject.SetActive(false);
        }

        public void InitMapPuzzle(GameFrameworkAction onComplatre)
        {
            OnComplatre = onComplatre;
            gameObject.SetActive(true);
        }

        private void InitTopic()
        {
            GameObject[][] touchArray = null;
            if (Id == 1 && m_DifficultyType == DifficultyType.Easy)
            {
                touchArray = new GameObject[][]
                {
                    new GameObject[]{m_TouchObjs[1], m_TouchObjs[4], },
                    new GameObject[]{m_TouchObjs[2], m_TouchObjs[7], },
                    new GameObject[]{m_TouchObjs[8], m_TouchObjs[12], },
                    new GameObject[]{m_TouchObjs[11], m_TouchObjs[15], },
                };
            }
            else if (Id == 2 && m_DifficultyType == DifficultyType.Easy)
            {
                touchArray = new GameObject[][]
                {
                    new GameObject[]{m_TouchObjs[0], m_TouchObjs[3], },
                    new GameObject[]{m_TouchObjs[4], m_TouchObjs[13], },
                    new GameObject[]{m_TouchObjs[7], m_TouchObjs[14], },
                };
            }
            else if (Id == 3 && m_DifficultyType == DifficultyType.Easy)
            {
                touchArray = new GameObject[][]
                {
                    new GameObject[]{m_TouchObjs[1], m_TouchObjs[12], },
                    new GameObject[]{m_TouchObjs[2], m_TouchObjs[15], },
                };
            }
            else if (Id == 4 && m_DifficultyType == DifficultyType.Easy)
            {
                touchArray = new GameObject[][]
                {
                    new GameObject[]{m_TouchObjs[1], m_TouchObjs[4], },
                    new GameObject[]{m_TouchObjs[2], m_TouchObjs[7], },
                    new GameObject[]{m_TouchObjs[8], m_TouchObjs[13], },
                    new GameObject[]{m_TouchObjs[11], m_TouchObjs[14], },
                };
            }
            else if (Id == 5 && m_DifficultyType == DifficultyType.Easy)
            {
                touchArray = new GameObject[][]
                {
                    new GameObject[]{m_TouchObjs[2], m_TouchObjs[8], },
                    new GameObject[]{m_TouchObjs[3] },
                    new GameObject[]{m_TouchObjs[12] },
                    new GameObject[]{m_TouchObjs[7], m_TouchObjs[13], },
                };
            }
            else if (Id == 1 && m_DifficultyType == DifficultyType.Trouble)
            {
                touchArray = new GameObject[][]
                {
                    new GameObject[]{m_TouchObjs[1], m_TouchObjs[3], },
                    new GameObject[]{m_TouchObjs[5], m_TouchObjs[10], },
                    new GameObject[]{m_TouchObjs[9], m_TouchObjs[14], },
                    new GameObject[]{m_TouchObjs[15], m_TouchObjs[21], },
                    new GameObject[]{m_TouchObjs[19], m_TouchObjs[23], },
                };
            }
            else if (Id == 2 && m_DifficultyType == DifficultyType.Trouble)
            {
                touchArray = new GameObject[][]
                {
                    new GameObject[]{m_TouchObjs[1], m_TouchObjs[5], },
                    new GameObject[]{m_TouchObjs[3], m_TouchObjs[9], },
                    new GameObject[]{m_TouchObjs[15], m_TouchObjs[21], },
                    new GameObject[]{m_TouchObjs[19], m_TouchObjs[23], },
                };
            }
            else if (Id == 3 && m_DifficultyType == DifficultyType.Trouble)
            {
                touchArray = new GameObject[][]
                {
                    new GameObject[]{m_TouchObjs[1], m_TouchObjs[5], },
                    new GameObject[]{m_TouchObjs[3], m_TouchObjs[9], },
                    new GameObject[]{m_TouchObjs[15], m_TouchObjs[21], },
                    new GameObject[]{m_TouchObjs[19], m_TouchObjs[23], },
                };
            }
            else if (Id == 4 && m_DifficultyType == DifficultyType.Trouble)
            {
                touchArray = new GameObject[][]
                {
                    new GameObject[]{m_TouchObjs[0], m_TouchObjs[6], m_TouchObjs[16], m_TouchObjs[20], },
                    new GameObject[]{m_TouchObjs[4], m_TouchObjs[8], m_TouchObjs[18], m_TouchObjs[24], },
                };
            }
            else if (Id == 5 && m_DifficultyType == DifficultyType.Trouble)
            {
                touchArray = new GameObject[][]
                {
                    new GameObject[]{m_TouchObjs[1], m_TouchObjs[5], },
                    new GameObject[]{m_TouchObjs[2], m_TouchObjs[14], },
                    new GameObject[]{m_TouchObjs[10], m_TouchObjs[22], },
                    new GameObject[]{m_TouchObjs[19], m_TouchObjs[23], },
            };
            }

            m_MapPuzzleDatas = new MapPuzzleData[m_EdgeImages.Length];
            for (int i = 0; i < m_EdgeImages.Length; i++)
            {
                m_MapPuzzleDatas[i] = new MapPuzzleData(m_EdgeImages[i], touchArray[i]);
            }
            m_PropFlags = new bool[m_PropTouchObjs.Length];
            for (int i = 0; i < m_PropFlags.Length; i++)
            {
                m_PropFlags[i] = false;
            }
        }

        private void ComplateFirst()
        {
            m_IsPropComplete = true;
            m_CompleteImage.enabled = true;
            m_CompleteImage.color = Color.white;
            StartCoroutine(IEnumeratorSoundShake(3,(int)UISoundId.VoiceShake,0.5f));
            m_CompleteImage.DOFade(0, 0.5f).SetLoops(3, LoopType.Yoyo).SetEase(Ease.Linear).OnComplete(() =>
            {
                m_CompleteImage.color = Color.white;
                m_IsPropComplete = false;
            });
        }

       

        private bool IsAllComplete()
        {
            if (m_MapPuzzleDatas == null) return false;
            for (int i = 0; i < m_MapPuzzleDatas.Length; i++)
            {
                if (!m_MapPuzzleDatas[i].IsComplete)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsAllPropComplete()
        {
            if (m_PropFlags == null) return false;
            for (int i = 0; i < m_PropFlags.Length; i++)
            {
                if (!m_PropFlags[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void Scale(GameObject obj)
        {
            obj.transform.DOScale(1, 0.5f).SetEase(Ease.Linear);

        }

    }
}
