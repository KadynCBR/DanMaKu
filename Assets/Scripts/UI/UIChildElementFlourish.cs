using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIChildElementFlourish : MonoBehaviour
{
    public float slideDistance = 100f;
    public float slideTime = .5f;
    public float delayBetween = .1f;

    public void OnEnable()
    {
        Activate();
    }

    public void Activate()
    {
        float f = 0;
        foreach (Transform child in transform)
        {
            LeftSlideFade(child, f);
            f += delayBetween;
        }
    }

    void LeftSlideFade(Transform trans, float delaytime)
    {
        RectTransform rt = trans.GetComponent<RectTransform>();
        TextMeshProUGUI txt = trans.GetComponent<TextMeshProUGUI>();
        float originalx = rt.anchoredPosition.x;
        Debug.Log($"orix {originalx}");
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + slideDistance, rt.anchoredPosition.y);
        Debug.Log($"newx {rt.anchoredPosition.x}");
        txt.alpha = 0;
        rt.DOAnchorPosX(originalx, slideTime).SetDelay(delaytime);
        txt.DOFade(1, slideTime).SetDelay(delaytime);

    }
}
