using UnityEngine;
using System.Collections;
using System;

namespace Penny
{
    public class KinectUtility
    {
        //获取视口坐标，即屏幕坐标
        public static Vector3 GetViewportToScreenPoint(KinectInterop.JointType jointType, Camera main)
        {
            KinectManager _manager = KinectManager.Instance;

            if (_manager && _manager.IsInitialized())//如果初始化成功
            {
                //先检查人物是否被检测
                if (_manager.IsUserDetected())
                {
                    //获取人物ID
                    long userId = _manager.GetPrimaryUserID();
                    //关节索引
                    int jointIndex = (int)jointType;
                    //判断需要跟踪的关节点是否已经被识别
                    if (_manager.IsJointTracked(userId, jointIndex))
                    {
                        Vector3 jointKinectPos = _manager.GetJointKinectPosition(userId, jointIndex);
                        if (jointKinectPos != Vector3.zero)
                        {

                            Vector2 posDepth = _manager.MapSpacePointToDepthCoords(jointKinectPos);
                            ushort depthValue = _manager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);
                            if (depthValue > 0)
                            {
                                Vector2 posColor = _manager.MapDepthPointToColorCoords(posDepth, depthValue);
                                float xNorm = posColor.x / _manager.GetColorImageWidth();
                                float yNorm = 1.0f - posColor.y / _manager.GetColorImageHeight();

                                Vector3 vpos = main.ViewportToScreenPoint(new Vector3(xNorm, yNorm, 0));
                                return vpos;
                            }
                        }
                    }
                }
            }
            return Vector3.zero;
        }

        //根据颜色图像和视口矩形获取骨骼节点位置
        public static Vector3 GetJointPosColorOverlay(int playerIndex, int iJointIndex, Camera camera)
        {
            KinectManager manager = KinectManager.Instance;

            if (manager && manager.IsInitialized() && camera)
            {
                // 得到背景矩形(使用肖像的背景,如果可用)
                Rect backgroundRect = camera.pixelRect;
                PortraitBackground portraitBack = PortraitBackground.Instance;
                if (portraitBack && portraitBack.enabled)
                {
                    backgroundRect = portraitBack.GetBackgroundRect();
                }
                // overlay the joint
                if (manager.IsUserDetected())
                {
                    long userId = manager.GetUserIdByIndex(playerIndex);

                    if (manager.IsJointTracked(userId, iJointIndex))
                    {
                        Vector3 posJoint = manager.GetJointPosColorOverlay(userId, iJointIndex, camera, backgroundRect);
                        return posJoint;
                    }
                }
            }
            return Vector3.zero;
        }
    }
}
