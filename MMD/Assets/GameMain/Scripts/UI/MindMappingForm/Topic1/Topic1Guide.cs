using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GameFramework;

namespace Penny
{
    public class Topic1Guide : GuideBase
    {
        // 1_yanshi_pini_1        皮尼：哦 ~太糟糕了~我要赶紧把东西收拾整齐
        // 1_yanshi_pangbai_2     旁白：皮尼想把零食放到从下面数第一个抽屉
        // 1_yanshi_pini_3        皮尼：用手拍一下零食
        // 1_yanshi_pini_4        皮尼：然后再打开柜子
        // 1_yanshi_pini_5        皮尼：看 ~零食就放好了~嘿嘿嘿~

        protected override IEnumerator IEnumeratorGuide(Transform fingerTransform, Transform slapTransform, GameFrameworkAction firstEvent, GameFrameworkAction secondEvent,Animator penyAnimator)
        {
            Init();
            SetFinger(fingerTransform.position);
            SetSlap(fingerTransform.position);
            GameEntry.Sound.StopSound(s_GuideSoundId);
            //皮尼：哦 ~太糟糕了~我要赶紧把东西收拾整齐
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_yanshi_pini_1, ref m_ClipMaxLength);
            penyAnimator.SetInteger("state", 6);
            yield return new WaitForSeconds(m_ClipMaxLength);
            penyAnimator.SetInteger("state",0);
            //旁白：皮尼想把零食放到从下面数第一个抽屉
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_yanshi_pangbai_2, ref m_ClipMaxLength);
            yield return new WaitForSeconds(m_ClipMaxLength);
            // 皮尼：用手拍一下零食
            penyAnimator.SetInteger("state", 6);
            FirstFinger(firstEvent);
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_yanshi_pini_3, ref m_ClipMaxLength);
            yield return new WaitForSeconds(m_ClipMaxLength);
            penyAnimator.SetInteger("state", 0);
            yield return new WaitForSeconds(4);
           
            while (Slap.gameObject.activeSelf)
            {
                yield return new WaitForEndOfFrame();
            }
            // 显示引导线
            SetSlap(slapTransform.position);
            GameEntry.Sound.PlayUISound((int)UISoundId.GuidePoint);
            yield return Line(Finger.transform, Slap.transform);
            penyAnimator.SetInteger("state", 6);
            // 皮尼：然后再打开柜子
            SecondFinger(secondEvent);
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_yanshi_pini_4, ref m_ClipMaxLength);
            yield return new WaitForSeconds(m_ClipMaxLength);
            penyAnimator.SetInteger("state", 0);
            while (Slap.gameObject.activeSelf)
            {
                yield return new WaitForEndOfFrame();
            }
            penyAnimator.SetInteger("state", 6);
            // 皮尼：看 ~零食就放好了~嘿嘿嘿~
            s_GuideSoundId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_yanshi_pini_5, ref m_ClipMaxLength);
            yield return new WaitForSeconds(m_ClipMaxLength);
            penyAnimator.SetInteger("state", 0);

            yield return new WaitForSeconds(0.5f);

            End();
        }
    }

}
