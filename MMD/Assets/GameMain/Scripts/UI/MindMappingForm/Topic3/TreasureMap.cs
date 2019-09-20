using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Penny
{
    public class TreasureMap : TopicTouchBase
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
        private GraphicalType m_GraphicalType = GraphicalType.None;
        [SerializeField]
        private ColorType m_ColorType = ColorType.None;
        [SerializeField]
        private Image m_GraphicalImage = null;
        [SerializeField]
        private Image m_ShovelImage = null;

        public GraphicalType GraphicalType
        {
            get { return m_GraphicalType; }
        }

        public ColorType ColorType
        {
            get { return m_ColorType; }
        }

        protected override void Awake()
        {
            base.Awake();
            //Id = (int)m_GraphicalType;
            m_ShovelImage.enabled = false;
            Clear();
        }

        protected override void Start()
        {

        }

        /// <summary>
        /// 设置藏宝图
        /// </summary>
        /// <param name="graphicalSprite"></param>
        /// <param name="color"></param>
        /// <param name="graphical"></param>
        public void Topic(Sprite graphicalSprite, ColorType color, GraphicalType graphical)
        {
            m_ColorType = color;
            Correct(graphicalSprite, graphical);
        }

        /// <summary>
        /// 修正
        /// </summary>
        /// <param name="graphicalSprite"></param>
        /// <param name="graphical"></param>
        public void Correct(Sprite graphicalSprite, GraphicalType graphical)
        {
            m_GraphicalImage.sprite = graphicalSprite;
            m_GraphicalImage.enabled = true;
            m_GraphicalType = graphical;
            Id = (int)graphical;
        }

        protected override void SelectSuccess()
        {
            base.SelectSuccess();
            ShovelShake();
        }

        public void SelectError()
        {
            if (!IsCanTouch) return;
            IsCanTouch = false;
            if (m_GraphicalType == GraphicalType.None && m_ShovelImage.enabled)
            {
                SelectFailed();
            }
        }

        private void ShovelShake()
        {
            m_ShovelImage.transform.DOShakePosition(1, new Vector3(10, 0, 0)).OnComplete(() =>
            {
                m_ShovelImage.enabled = false;
                m_GraphicalImage.enabled = true;
            });
        }

        public override void Clear()
        {
            m_GraphicalImage.enabled = false;
            m_ShovelImage.enabled = false;
            m_GraphicalType = GraphicalType.None;
            m_ColorType = ColorType.None;
        }
    }
}