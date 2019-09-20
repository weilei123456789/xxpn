using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace Penny
{
    public class Topic2Guide : GuideBase
    {
//10024	太精彩了 ~哇喔~    2_yanshi_pini_2
//10025	哦 ~看，冠军出现了	2_yanshi_pangbai_3
//10026	我要给冠军赛车涂上红色，哈哈哈	2_yanshi_pini_4
//10027	用手拍一下红色油漆桶	2_yanshi_pini_5
//10028	然后再涂到赛车上	2_yanshi_pini_6
//10029	看 ~超酷的赛车~嘿嘿嘿~    2_yanshi_pini_7


        protected override IEnumerator IEnumeratorGuide(Transform fingerTransform, Transform slapTransform, GameFrameworkAction firstEvent, GameFrameworkAction secondEvent,Animator penyAnimator)
        {
            Init();
            SetFinger(fingerTransform.position);
            SetSlap(fingerTransform.position);
            GameEntry.Sound.StopSound(s_GuideSoundId);
            //太精彩了 ~哇喔~
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s2_yanshi_pini_2, ref m_ClipMaxLength);
            yield return new WaitForSeconds(m_ClipMaxLength);
            //哦 ~看，冠军出现了
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s2_yanshi_pangbai_3, ref m_ClipMaxLength);
            yield return new WaitForSeconds(m_ClipMaxLength);
            // 我要给冠军赛车涂上红色，哈哈哈
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s2_yanshi_pini_4, ref m_ClipMaxLength);
            yield return new WaitForSeconds(m_ClipMaxLength);
            // 用手拍一下红色油漆桶
            FirstFinger(firstEvent);
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s2_yanshi_pini_5, ref m_ClipMaxLength);
            yield return new WaitForSeconds(m_ClipMaxLength + 4);
            while (Slap.gameObject.activeSelf)
            {
                yield return new WaitForEndOfFrame();
            }
            // 显示引导线
            SetSlap(slapTransform.position);
            yield return Line(Finger.transform, Slap.transform);
            // 然后再涂到赛车上
            SecondFinger(secondEvent);
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s2_yanshi_pini_6, ref m_ClipMaxLength);
            yield return new WaitForSeconds(m_ClipMaxLength);
            while (Slap.gameObject.activeSelf)
            {
                yield return new WaitForEndOfFrame();
            }
            // 皮尼：看 ~零食就放好了~嘿嘿嘿~
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s2_yanshi_pini_7, ref m_ClipMaxLength);
            yield return new WaitForSeconds(m_ClipMaxLength);
            End();
        }
    }

}
