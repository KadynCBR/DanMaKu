using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum EntryMode
{
    DO_NOTHING,
    SLIDE,
    ZOOM,
    FADE,
}

public enum UIDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NONE
}

// inital design pulled from https://www.youtube.com/watch?v=9MIwIaRUUhc

[RequireComponent(typeof(AudioSource), typeof(CanvasGroup))]
[DisallowMultipleComponent]
public class Page : MonoBehaviour
{
    private AudioSource audioSource;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float AnimationSpeed = 1f;
    public bool ExitOnNewPagePush = false;
    [SerializeField]
    private AudioClip EntryClip;
    [SerializeField]
    private AudioClip ExitClip;
    [SerializeField]
    private EntryMode entryMode = EntryMode.SLIDE;
    [SerializeField]
    private UIDirection EntryDirction = UIDirection.LEFT;
    [SerializeField]
    private EntryMode ExitMode = EntryMode.SLIDE;
    [SerializeField]
    private UIDirection ExitDirection = UIDirection.LEFT;
    [SerializeField]
    private UnityEvent prePushAction;
    [SerializeField]
    private UnityEvent postPushAction;
    [SerializeField]
    private UnityEvent prePopAction;
    [SerializeField]
    private UnityEvent postPopAction;
    [SerializeField]
    private GameObject firstFocusItemOnPage;

    private Coroutine AnimationCoroutine;
    private Tween AnimationTween;
    private Coroutine AudioCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0;
        audioSource.enabled = false;
        canvasGroup.interactable = false;
    }

    public void Enter(bool playAudio)
    {
        prePushAction?.Invoke();
        canvasGroup.interactable = true;
        Debug.Log("entering!");
        if (firstFocusItemOnPage != null)
        {
            Debug.Log($"{this.name}: entr Setting selected game object: [{firstFocusItemOnPage.name}]");
            EventSystem.current.SetSelectedGameObject(firstFocusItemOnPage);
        }
        switch (entryMode)
        {
            case EntryMode.SLIDE:
                Slide(playAudio, true);
                break;
            case EntryMode.ZOOM:
                Zoom(playAudio, true);
                break;
            case EntryMode.FADE:
                Fade(playAudio, true);
                break;

        }
    }

    // If we didn't exit on page exit, we're reentering. set selected.
    public void Reenter()
    {
        Debug.Log("Reentering!");
        if (firstFocusItemOnPage != null)
        {
            Debug.Log($"{this.name}: reentr Setting selected game object: [{firstFocusItemOnPage.name}]");
            EventSystem.current.SetSelectedGameObject(firstFocusItemOnPage);
        }
    }

    public void Exit(bool playAudio)
    {
        prePopAction?.Invoke();
        canvasGroup.interactable = false;
        switch (ExitMode)
        {
            case EntryMode.SLIDE:
                Slide(playAudio, false);
                break;
            case EntryMode.ZOOM:
                Zoom(playAudio, false);
                break;
            case EntryMode.FADE:
                Fade(playAudio, false);
                break;

        }
    }

    // See if we can use DoTween for these.
    private void Fade(bool playAudio, bool isEntry)
    {
        if (AnimationTween != null && AnimationTween.IsActive() && AnimationTween.IsPlaying())
        {
            AnimationTween.Kill();
        }
        if (isEntry)
        {
            AnimationTween = canvasGroup.DOFade(1, AnimationSpeed).SetUpdate(true).OnComplete(() => { canvasGroup.blocksRaycasts = true; postPushAction?.Invoke(); });
        }
        else
        {
            AnimationTween = canvasGroup.DOFade(0, AnimationSpeed).SetUpdate(true).OnComplete(() => { canvasGroup.blocksRaycasts = false; postPopAction?.Invoke(); });
        }
        PlayTransitionClip(playAudio, isEntry);
    }

    private void Zoom(bool playAudio, bool isEntry)
    {
        if (AnimationTween != null && AnimationTween.IsActive() && AnimationTween.IsPlaying())
        {
            AnimationTween.Kill();
        }
        if (isEntry)
        {
            rectTransform.localScale = Vector2.zero;
            AnimationTween = rectTransform.DOScale(Vector3.one, AnimationSpeed).SetUpdate(true).OnComplete(() => { postPushAction?.Invoke(); });
        }
        else
        {
            AnimationTween = rectTransform.DOScale(Vector3.zero, AnimationSpeed).SetUpdate(true).OnComplete(() => { postPopAction?.Invoke(); });
        }
        PlayTransitionClip(playAudio, isEntry);
    }

    private void Slide(bool playAudio, bool isEntry)
    {
        if (AnimationTween != null && AnimationTween.IsActive() && AnimationTween.IsPlaying())
        {
            AnimationTween.Kill();
        }
        if (isEntry)
        {
            rectTransform.anchoredPosition = DirectionToVector2(EntryDirction);
            AnimationTween = rectTransform.DOLocalMove(Vector3.zero, AnimationSpeed).SetUpdate(true).OnComplete(() => { postPushAction?.Invoke(); });
        }
        else
        {
            AnimationTween = rectTransform.DOLocalMove(DirectionToVector2(ExitDirection), AnimationSpeed).SetUpdate(true).OnComplete(() => { postPopAction?.Invoke(); });
        }
        PlayTransitionClip(playAudio, isEntry);
    }

    private void PlayTransitionClip(bool playAudio, bool isEntry)
    {
        AudioClip c;
        if (isEntry) c = EntryClip;
        else c = ExitClip;

        if (playAudio && c != null && audioSource != null)
        {
            if (AudioCoroutine != null)
            {
                StopCoroutine(AudioCoroutine);
            }
            AudioCoroutine = StartCoroutine(PlayClip(c));
        }
    }

    private IEnumerator PlayClip(AudioClip clip)
    {
        audioSource.enabled = true;
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        audioSource.enabled = false;
    }

    private Vector2 DirectionToVector2(UIDirection dir)
    {
        switch (dir)
        {
            case UIDirection.UP:
                return new Vector2(0, Screen.height);
            case UIDirection.DOWN:
                return new Vector2(0, -Screen.height);
            case UIDirection.LEFT:
                return new Vector2(-Screen.width, 0);
            case UIDirection.RIGHT:
                return new Vector2(Screen.width, 0);
        }
        return new Vector2(0, 0);
    }

}
