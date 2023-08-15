using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum WORLDSHIFTCOLOR
{
    BLACK,
    WHITE
}

public class WorldShifter : MonoBehaviour
{
    public Transform bhold;
    public Transform whold;

    public float aboveVal;
    public float belowVal;
    public bool currentlyShifting;

    public WORLDSHIFTCOLOR currentShift;


    void Start()
    {
        bhold = GameObject.FindGameObjectWithTag("bshift").transform;
        whold = GameObject.FindGameObjectWithTag("wshift").transform;
        aboveVal = bhold.position.y;
        belowVal = whold.position.y;
        currentShift = WORLDSHIFTCOLOR.BLACK;
    }

    public void OnWorldShift()
    {
        if (currentlyShifting) return;
        currentlyShifting = true;
        // Maybe this would be better attached to one of the items below.
        DOVirtual.DelayedCall(3.1f, () => { currentlyShifting = false; });
        if (currentShift == WORLDSHIFTCOLOR.BLACK)
        {
            whold.DOMoveY(aboveVal, 3f).SetEase(Ease.InOutBounce).SetUpdate(true);
            bhold.DOMoveY(belowVal, 3f).SetEase(Ease.InOutBounce).SetUpdate(true);
            currentShift = WORLDSHIFTCOLOR.WHITE;
        }
        else
        {
            whold.DOMoveY(belowVal, 3f).SetEase(Ease.InOutBounce).SetUpdate(true);
            bhold.DOMoveY(aboveVal, 3f).SetEase(Ease.InOutBounce).SetUpdate(true);
            currentShift = WORLDSHIFTCOLOR.BLACK;
        }
    }

    // public void OnSlowTime()
    // {
    //     StartCoroutine(TimeSlow(3f));
    // }

    // IEnumerator TimeSlow(float duration)
    // {
    //     float evalPoint;
    //     timeexplode_sfx.timeSamples = 0;
    //     timeexplode_sfx.Play();
    //     // Need to use unscaled time since everything will slow?
    //     for (float i = 0; i < duration / 2; i += Time.unscaledDeltaTime)
    //     {
    //         evalPoint = Mathf.Lerp(1f, 0f, i / (duration / 2));
    //         Time.timeScale = timeexplode_sfx.pitch = bgm.pitch = timeCurve.Evaluate(evalPoint);
    //         yield return null;
    //     }

    //     yield return new WaitForSecondsRealtime(.25f);
    //     timeexplode_sfx.timeSamples = timeexplode_sfx.clip.samples - 1;
    //     timeexplode_sfx.Play();

    //     for (float i = 0; i < duration / 2; i += Time.unscaledDeltaTime)
    //     {
    //         evalPoint = Mathf.Lerp(0f, 1f, i / (duration / 2));
    //         Time.timeScale = bgm.pitch = timeCurve.Evaluate(evalPoint);
    //         timeexplode_sfx.pitch = -Time.timeScale;
    //         yield return null;
    //     }
    //     Time.timeScale = timeexplode_sfx.pitch = bgm.pitch = 1;
    // }
}
