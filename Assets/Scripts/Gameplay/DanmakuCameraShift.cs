using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DanmakuCameraShift : MonoBehaviour
{
    private CinemachineVirtualCamera cvc;

    void Awake()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
    }


    public void ToggleDanmakuMode()
    {
        if (cvc.Priority == 17)
        {
            cvc.Priority = 0;
        }
        else
        {
            cvc.Priority = 17;
        }
    }
}
