using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LerpPairs
{
    public Transform Actual;
    public Transform Target;
    public float distance;
}

public class LerpToTargetOrdered : MonoBehaviour
{
    public List<LerpPairs> Lerpies;
    private LerpPairs Furthest;
    public float distanceUntilLerp;
    public float lerpSpeed;
    public bool isLerping;
    public float liftDistance;
    public float overshoot;

    void FixedUpdate()
    {
        Furthest = null;
        foreach (LerpPairs lp in Lerpies)
        {
            lp.distance = Vector3.Distance(lp.Actual.position, lp.Target.position);
            if (lp.distance > distanceUntilLerp)
            {
                if (Furthest == null)
                    Furthest = lp;
                if (lp.distance > Furthest.distance)
                    Furthest = lp;
            }
        }
        if (isLerping || Furthest == null) return;
        StartCoroutine(LerpLerp(Furthest.Actual, Furthest.Target, lerpSpeed));
    }

    IEnumerator LerpLerp(Transform actual, Transform target, float duration)
    {
        isLerping = true;
        Vector3 fromPos = actual.position;
        Vector3 toPos = target.position;

        toPos = target.position + ((target.position - actual.position).normalized * overshoot);

        Vector3 center = (fromPos + toPos) * .5f;
        // move center down to maker arc vertical?
        center -= Vector3.up * liftDistance;

        // change from and pos
        Vector3 fromRelCenter = fromPos - center;
        Vector3 toRelCenter = toPos - center;

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            yield return null;
            actual.position = Vector3.Slerp(fromRelCenter, toRelCenter, t);
            actual.position += center;
        }
        actual.position = toPos;
        isLerping = false;
    }
}
