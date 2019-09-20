using DG.Tweening;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penny
{
    public class Car : TopicTouchBase
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

        [SerializeField]
        private Image m_ColorMask = null;

        [SerializeField]
        private TeamType m_TeamType = TeamType.None;

        public TeamType Team
        {
            get { return m_TeamType; }
        }

        protected override void Start()
        {
            Clear();
        }

        public override bool Select(PropType subProp, TeamType teamType, int id, PaintDrum paint_drum, GameFrameworkAction finish)
        {
            if (!IsCanTouch) return false;
            IsCanTouch = false;
            //true
            if (subProp == PropType && Id == id && m_TeamType == teamType)
            {
                SelectSuccess();
                MoveTo(paint_drum, finish);
                return true;
            }
            // false
            else
            {
                SelectFailed();
                return false;
            }
        }

        /// <summary>
        /// 移动笔刷
        /// </summary>
        /// <param name="paint_drum"></param>
        /// <param name="finish"></param>
        public void MoveTo(PaintDrum paint_drum, GameFrameworkAction finish)
        {
            paint_drum.BrushStopShake();
            Sequence ms = DOTween.Sequence();
            Tween move1 = paint_drum.BrushTransform.DOMove(transform.position + Vector3.left, 1).SetEase(Ease.Linear);
            Tween move2 = paint_drum.BrushTransform.DOMoveX(transform.position.x + 1, 0.625f).SetLoops(4, LoopType.Yoyo);
            StartCoroutine(IEnumeratorBrush());
            move2.OnComplete(() =>
            {
                if (finish != null) finish();
                m_ColorMask.enabled = true;
                paint_drum.InitBrush();
            });
            ms.Append(move1);
            ms.Append(move2);
        }

        protected IEnumerator IEnumeratorBrush()
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < 4; i++)
            {
                GameEntry.Sound.PlayUISound((int)UISoundId.Brush);
                yield return new WaitForSeconds(0.625f);
            }

        }

        protected override void SelectSuccess()
        {
            base.SelectSuccess();
            //Discoloration(Color.yellow);
            //StartAutoScale();
            //m_ColorMask.enabled = true;
        }

        public override void Clear()
        {
            base.Clear();
            m_ColorMask.enabled = false;
        }
    }
}