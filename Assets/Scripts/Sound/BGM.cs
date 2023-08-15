using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
// using Sirenix.OdinInspector;

public class BGM : MonoBehaviour
{
    public AudioSource source;
    public float volume;
    public List<AudioClip> tracks;
    public int currentTrackIndex;
    public AudioClip currentAudioClip;

    // [Button]
    public void changeTrack(AudioClip newTrack, float fadeTime = 1f)
    {
        if (currentAudioClip == newTrack) return;
        currentAudioClip = newTrack;
        StopAllCoroutines();
        StartCoroutine(CrossFadeBGM(newTrack, fadeTime));
    }


    // Need to figure out which one of these I'm gunna use.
    public void changeTrack(int trackNum, float fadeTime = 1f)
    {
        changeTrack(tracks[trackNum], fadeTime);
    }

    public void changeTrack(int trackNum)
    {
        changeTrack(trackNum, .25f);
    }

    public IEnumerator CrossFadeBGM(AudioClip newTrack, float fadeTime)
    {
        for (float i = 0; i < fadeTime; i += Time.unscaledDeltaTime / fadeTime)
        {
            source.volume -= volume * Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }

        source.clip = newTrack;

        source.Play();

        for (float i = 0; i < fadeTime; i += Time.unscaledDeltaTime / fadeTime)
        {
            source.volume += volume * Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }
    }
}
