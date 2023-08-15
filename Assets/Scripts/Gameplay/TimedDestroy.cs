using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimedDestroy : MonoBehaviour
{
    [SerializeField]
    private float destroyDelay;
    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}
