using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimedDestroyComponent : MonoBehaviour
{
    public Component ComponentToDestroy;
    public float delay;

    void Start()
    {
        Destroy(ComponentToDestroy, delay);
        Destroy(this, delay);
    }
}
