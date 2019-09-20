using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

namespace Penny
{
    public class Shovel : MonoBehaviour
    {
        [SerializeField]
        private Image m_ShovelImage = null;

        [SerializeField]
        private Sprite m_ShovelSprite = null;

        /// <summary>
        /// 宝藏的图片
        /// </summary>
        [SerializeField]
        private Sprite[] m_RropSprite = null;


        public void ShovelShake(Vector3 position)
        {
            m_ShovelImage.enabled = true;
            m_ShovelImage.transform.position = position;
            m_ShovelImage.sprite = m_ShovelSprite;

            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < 3; i++)
            {
                Tween move1 = m_ShovelImage.transform.DOLocalMove(m_ShovelImage.transform.localPosition + new Vector3(30, 30, 0), 0.33f);
                Tween move2 = m_ShovelImage.transform.DOLocalMove(m_ShovelImage.transform.localPosition, 0.33f);
                sequence.Append(move1);
                sequence.Append(move2);
            }
            Tween move3 = m_ShovelImage.transform.DOLocalMove(m_ShovelImage.transform.localPosition, 0.1f).OnComplete(() =>
            {
                int index = Utility.Random.GetRandom(0, m_RropSprite.Length);
                m_ShovelImage.sprite = m_RropSprite[index];
            });
            sequence.Append(move3);
            Tween move4 = m_ShovelImage.transform.DOLocalMoveY(-510, 1);
            Tween scale = m_ShovelImage.transform.DOScale(1.5f, 1);
            sequence.Append(move4);
            sequence.Join(scale);
        }
    }
}