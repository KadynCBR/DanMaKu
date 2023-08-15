using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource timeexplode_sfx;
    public AnimationCurve timeCurve;

    public void OnSlowTime()
    {
        StartCoroutine(SlowTimeCO(3f));
    }

    IEnumerator SlowTimeCO(float duration)
    {
        float evalPoint;
        timeexplode_sfx.timeSamples = 0;
        timeexplode_sfx.Play();
        // Need to use unscaled time since everything will slow?
        for (float i = 0; i < duration / 2; i += Time.unscaledDeltaTime)
        {
            evalPoint = Mathf.Lerp(1f, 0f, i / (duration / 2));
            Time.timeScale = timeexplode_sfx.pitch = bgm.pitch = timeCurve.Evaluate(evalPoint);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(.25f);
        timeexplode_sfx.timeSamples = timeexplode_sfx.clip.samples - 1;
        timeexplode_sfx.Play();

        for (float i = 0; i < duration / 2; i += Time.unscaledDeltaTime)
        {
            evalPoint = Mathf.Lerp(0f, 1f, i / (duration / 2));
            Time.timeScale = bgm.pitch = timeCurve.Evaluate(evalPoint);
            timeexplode_sfx.pitch = -Time.timeScale;
            yield return null;
        }
        Time.timeScale = timeexplode_sfx.pitch = bgm.pitch = 1;
    }
}
