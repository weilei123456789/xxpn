using DG.Tweening;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penny
{
    public class CountDown : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_BackgroundObj = null;
        [SerializeField]
        private GameObject m_GoObj = null;

        [SerializeField]
        private Image m_CountDownImage = null;

        [SerializeField]
        private Sprite m_ThreeSprite = null;

        [SerializeField]
        private Sprite m_TwoSprite = null;

        [SerializeField]
        private Sprite m_OneSprite = null;

        [SerializeField]
        private UISpriteAnimation m_GoAnimation = null;

        private bool m_IsComplate = false;
        public bool IsComplate
        {
            get { return m_IsComplate; }
        }

        private void Start()
        {
            m_IsComplate = false;
            m_GoAnimation.FirstFrame();
        }

        public void StartCountDown(GameFrameworkAction OnComplate)
        {
            gameObject.SetActive(true);
            m_BackgroundObj.SetActive(true);
            m_GoObj.SetActive(false);
            m_IsComplate = false;
            StartCoroutine(IE_CountDown(OnComplate));
        }

        private IEnumerator IE_CountDown(GameFrameworkAction OnComplate)
        {
            m_CountDownImage.sprite = m_ThreeSprite;
            m_CountDownImage.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);
            GameEntry.Sound.PlayUISound((int)UISoundId.CountDown3);
            yield return new WaitForSeconds(1);
            m_CountDownImage.sprite = m_TwoSprite;
            m_CountDownImage.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);
            GameEntry.Sound.PlayUISound((int)UISoundId.CountDown2);
            yield return new WaitForSeconds(1);
            m_CountDownImage.sprite = m_OneSprite;
            m_CountDownImage.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);
            GameEntry.Sound.PlayUISound((int)UISoundId.CountDown1);
            
            yield return new WaitForSeconds(1);
            m_BackgroundObj.SetActive(false);
            m_GoObj.SetActive(true);
            m_GoAnimation.FirstFrame();
            GameEntry.Sound.PlayUISound((int)UISoundId.GO);
            yield return new WaitForSeconds(1);
            m_GoAnimation.Rewind(() =>
            {
                if (OnComplate != null)
                    OnComplate();
                gameObject.SetActive(false);
                m_IsComplate = true;
                GameEntry.Sound.PlayUISound((int)UISoundId.Begin);
            });
            yield break;
        }
    }
}
