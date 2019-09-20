using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UISpriteAnimation : MonoBehaviour
{
    public delegate void VoidDelegate();
    public VoidDelegate voidDelegate;
    private Image ImageSource;
    private int mCurFrame = 0;
    private float mDelta = 0;
    [HideInInspector]
    public float FPS = 30;
    public List<Sprite> SpriteFrames;
    public bool IsPlaying = false;
    public bool Foward = true;
    public bool AutoPlay = false;
    public bool Loop = false;
    public bool IsAutoSize = false;
    public int FrameCount
    {
        get
        {
            return SpriteFrames.Count;
        }
    }
    private Vector2[] UIPositions;

    void Awake()
    {
        ImageSource = GetComponent<Image>();
        if (AutoPlay)
        {
            Play();
        }
        else
        {
            IsPlaying = false;
        }
    }

    void Start()
    {

    }

    private void SetSprite(int idx)
    {
        ImageSource.sprite = SpriteFrames[idx];
        if (IsAutoSize)
            ImageSource.SetNativeSize();
    }

    public void Play()
    {
        IsPlaying = true;
        Foward = true;
    }

    public void PlayReverse()
    {
        IsPlaying = true;
        Foward = false;
    }

    void Update()
    {
        if (!IsPlaying || 0 == FrameCount)
        {
            return;
        }

        mDelta += Time.deltaTime;
        if (mDelta > 1 / FPS)
        {
            mDelta = 0;
            if (Foward)
            {
                mCurFrame++;
            }
            else
            {
                mCurFrame--;
            }

            if (mCurFrame >= FrameCount)
            {
                if (Loop)
                {
                    mCurFrame = 0;
                }
                else
                {
                    IsPlaying = false;
                    if (voidDelegate != null)
                        voidDelegate();
                    return;
                }
            }
            else if (mCurFrame < 0)
            {
                if (Loop)
                {
                    mCurFrame = FrameCount - 1;
                }
                else
                {
                    IsPlaying = false;
                    if (voidDelegate != null)
                        voidDelegate();
                    return;
                }
            }
            SetSprite(mCurFrame);
            if (UIPositions != null && UIPositions.Length > 0)
                transform.localPosition = UIPositions[mCurFrame];
        }
    }

    public void SetPositions(Vector2[] vector2s)
    {
        UIPositions = vector2s;
    }

    public void SetSprites(Sprite[] sprites)
    {
        SpriteFrames.Clear();
        SpriteFrames.AddRange(sprites);
    }

    public void FirstFrame()
    {
        mCurFrame = 0;
        SetSprite(0);
    }

    public void Pause()
    {
        IsPlaying = false;
    }

    public void Resume()
    {
        if (!IsPlaying)
            IsPlaying = true;
    }

    public void Stop()
    {
        mCurFrame = 0;
        SetSprite(mCurFrame);
        IsPlaying = false;
    }

    public void Rewind()
    {
        mCurFrame = 0;
        SetSprite(mCurFrame);
        Play();
    }

    public void Rewind(VoidDelegate void_delegate)
    {
        voidDelegate = void_delegate;
        mCurFrame = 0;
        SetSprite(mCurFrame);
        Play();
    }
}
