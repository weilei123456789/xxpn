using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Penny
{
    public class PaintDrum : TopicTouchBase
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
        private ColorType m_ColorType = ColorType.None;
        [SerializeField]
        private Image m_PaintDrumImage = null;
        [SerializeField]
        private Transform Brush = null;
        [SerializeField]
        private Sprite m_NormalSprite = null;
        [SerializeField]
        private Sprite m_RimLightSprite = null;

        private Vector3 m_InitBrushPos = Vector3.zero;

        public Transform BrushTransform
        {
            get
            {
                return Brush;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_InitBrushPos = Brush.localPosition;
            Id = (int)m_ColorType;
        }

        protected override void Start()
        {
            Clear();
        }

        protected override void SelectSuccess()
        {
            base.SelectSuccess();
            StartAutoScale();
            //Discoloration(Color.yellow);
            m_PaintDrumImage.sprite = m_RimLightSprite;
            BrushLoopShake();
        }

        public override void Clear()
        {
            base.Clear();
            m_PaintDrumImage.sprite = m_NormalSprite;
            Brush.localPosition = m_InitBrushPos;
        }

        public void BrushLoopShake()
        {
            Brush.DOShakePosition(1, Vector3.up * 10).SetLoops(-1);
        }

        public void BrushStopShake()
        {
            Brush.DOKill();
        }

        public void InitBrush()
        {
            Brush.localPosition = m_InitBrushPos;
        }
    }
}