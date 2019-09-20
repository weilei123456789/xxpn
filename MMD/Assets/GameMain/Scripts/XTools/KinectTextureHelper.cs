using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public static class KinectTextureHelper
    {

        public static Texture2D ScaleTextureCutOut(Texture2D originalTexture, int pos_x, int pos_y, float originalWidth, float originalHeight)
        {
            //Color[] pixels = new Color[(int)(originalWidth * originalHeight)];
            //要返回的新图
            Texture2D newTexture = new Texture2D(Mathf.CeilToInt(originalWidth), Mathf.CeilToInt(originalHeight));
            //批量获取点像素
            Color[] pixels = originalTexture.GetPixels(pos_x, pos_y, (int)originalWidth, (int)originalHeight);
            newTexture.SetPixels(pixels);
            newTexture.anisoLevel = 2;
            newTexture = FlipColor(newTexture, newTexture.width, newTexture.height);
            //newTexture.Apply();
            return newTexture;
        }


        public static Texture2D Base64ToTexter2d(byte[] base64)
        {
            int Width = KinectManager.Instance.GetColorImageWidth();
            int Height = KinectManager.Instance.GetColorImageHeight();
            Texture2D pic = new Texture2D(Width, Height);
            pic.LoadImage(base64);
            return pic;
        }

        private static Texture2D FlipColor(Texture2D texture, int width, int height)
        {
            Texture2D flipTexture = new Texture2D(width, height);
            for (int i = 0; i < height; i++)
            {
                flipTexture.SetPixels(0, i, width, 1, texture.GetPixels(0, height - i - 1, width, 1));
            }
            flipTexture.Apply();
            return flipTexture;
        }

        public static void OverlayJoint(Camera camera, long userId, int jointIndex, Transform overlayObj, Rect backgroundRect, int smoothFactor = 10)
        {
            if (KinectManager.Instance.IsJointTracked(userId, jointIndex))
            {
                Vector3 posJoint = KinectManager.Instance.GetJointKinectPosition(userId, jointIndex);
                if (posJoint != Vector3.zero)
                {
                    Vector2 posDepth = KinectManager.Instance.MapSpacePointToDepthCoords(posJoint);
                    ushort depthValue = KinectManager.Instance.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);

                    if (depthValue > 0)
                    {
                        Vector2 posColor = KinectManager.Instance.MapDepthPointToColorCoords(posDepth, depthValue);

                        float xNorm = posColor.x / KinectManager.Instance.GetColorImageWidth();
                        float yNorm = 1.0f - posColor.y / KinectManager.Instance.GetColorImageHeight();

                        if (overlayObj && camera)
                        {
                            float distanceToCamera = overlayObj.position.z - camera.transform.position.z;
                            posJoint = camera.ViewportToWorldPoint(new Vector3(xNorm, yNorm, distanceToCamera));
                            posJoint.z = 0;
                            overlayObj.position = Vector3.Lerp(overlayObj.position, posJoint, Time.deltaTime * smoothFactor);
                            //overlayObj.rotation = Quaternion.identity;
                        }
                    }
                }
            }
        }
    }

}