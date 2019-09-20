using UnityEngine;
using UnityEngine.UI;

namespace PaintTools
{

    public class PaintView : MonoBehaviour
    {
        //绘图shader&material
        [SerializeField]
        private Shader _paintBrushShader;
        private Material _paintBrushMat;
        //清理renderTexture的shader&material
        [SerializeField]
        private Shader _clearBrushShader;
        private Material _clearBrushMat;
        //默认笔刷&笔刷合集
        [SerializeField]
        private Texture _defaultBrushTex;
        //renderTexture
        private RenderTexture _renderTex;
        //绘画的画布
        [SerializeField]
        private RawImage _paintCanvas;
        //笔刷的默认颜色&颜色合集
        [SerializeField]
        private Color _defaultColor;
        //笔刷的大小
        [SerializeField]
        private float _brushSize = 300;
        //屏幕的宽高
        private int _screenWidth;
        private int _screenHeight;
        ////笔刷的间隔大小
        //private float _brushLerpSize;
        ////默认上一次点的位置
        //private Vector2 _lastPoint;

        void Start()
        {
            //_brushLerpSize = (_defaultBrushTex.width + _defaultBrushTex.height) / 2.0f / _brushSize;
            //_lastPoint = Vector2.zero;

            if (_paintBrushMat == null)
            {
                _paintBrushMat = new Material(_paintBrushShader);
                SetBrushTexture(_defaultBrushTex);
                SetBrushColor(_defaultColor);
                SetBrushSize(_brushSize);
            }
            if (_clearBrushMat == null)
                _clearBrushMat = new Material(_clearBrushShader);
            if (_renderTex == null)
            {
                _screenWidth = Screen.width;
                _screenHeight = Screen.height;

                _renderTex = RenderTexture.GetTemporary(_screenWidth, _screenHeight, 24);
                _paintCanvas.texture = _renderTex;
            }
            Graphics.Blit(null, _renderTex, _clearBrushMat);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Paint(Input.mousePosition);
            }
            //if (Input.GetMouseButtonUp(0))
            //    _lastPoint = Vector2.zero;
        }

        public void SetBrushSize(float size)
        {
            _brushSize = size;
            _paintBrushMat.SetFloat("_Size", _brushSize);
        }

        public void SetBrushTexture(Texture texture)
        {
            _defaultBrushTex = texture;
            _paintBrushMat.SetTexture("_BrushTex", _defaultBrushTex);
        }

        public void SetBrushColor(Color color)
        {
            _defaultColor = color;
            _paintBrushMat.SetColor("_Color", _defaultColor);
        }

        ////插点
        //public void LerpPaint(Vector2 point)
        //{
        //    Paint(point);

        //    if (_lastPoint == Vector2.zero)
        //    {
        //        _lastPoint = point;
        //        return;
        //    }

        //    float dis = Vector2.Distance(point, _lastPoint);
        //    if (dis > _brushLerpSize)
        //    {
        //        Vector2 dir = (point - _lastPoint).normalized;
        //        int num = (int)(dis / _brushLerpSize);
        //        for (int i = 0; i < num; i++)
        //        {
        //            Vector2 newPoint = _lastPoint + dir * (i + 1) * _brushLerpSize;
        //            Paint(newPoint);
        //        }
        //    }
        //    _lastPoint = point;
        //}

        //public void ClearPosition()
        //{
        //    _lastPoint = Vector2.zero;
        //}

        //画点
        public void Paint(Vector2 point)
        {
            if (point.x < 0 || point.x > _screenWidth || point.y < 0 || point.y > _screenHeight)
                return;
            Vector2 uv = new Vector2(point.x / (float)_screenWidth, point.y / (float)_screenHeight);
            _paintBrushMat.SetVector("_UV", uv);
            Graphics.Blit(_renderTex, _renderTex, _paintBrushMat);
        }

    }

}