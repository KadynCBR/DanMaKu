using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    public float duration;
    public float destroyDelay = .1f;

    void Start() { ActivateIndicator(); }

    void ActivateIndicator()
    {
        Image outerindicator = GetComponent<Image>();
        outerindicator.DOFade(.55f, .5f).SetLoops(Mathf.FloorToInt(duration / .5f), LoopType.Yoyo).SetEase(Ease.Linear);
        RectTransform innerRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        innerRectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        innerRectTransform.DOScale(Vector3.one, duration).SetEase(Ease.Linear);
        Destroy(gameObject, duration + destroyDelay);
    }
}
