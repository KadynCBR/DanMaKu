using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LerpToTarget : MonoBehaviour
{
    public Transform Actual;
    public Transform Target;
    public float distanceUntilLerp;
    public float lerpSpeed;
    public bool isLerping;
    public float liftDistance;
    public float overshoot;

    void FixedUpdate()
    {
        if (Vector3.Distance(Actual.position, Target.position) > distanceUntilLerp)
        {
            StartCoroutine(LerpLerp(lerpSpeed));
            // Actual.DOMove(Target.position, lerpSpeed);
        }
    }

    IEnumerator LerpLerp(float duration)
    {
        Vector3 fromPos = Actual.position;
        Vector3 toPos = Target.position;

        toPos = Target.position + ((Target.position - Actual.position).normalized * overshoot);

        Vector3 center = (fromPos + toPos) * .5f;
        // move center down to maker arc vertical?
        center -= Vector3.up * liftDistance;

        // change from and pos
        Vector3 fromRelCenter = fromPos - center;
        Vector3 toRelCenter = toPos - center;

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            yield return null;
            Actual.position = Vector3.Slerp(fromRelCenter, toRelCenter, t);
            Actual.position += center;
        }
        Actual.position = toPos;
    }
}
