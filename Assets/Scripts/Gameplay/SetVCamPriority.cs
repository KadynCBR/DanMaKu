using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetVCamPriority : MonoBehaviour
{
    private CinemachineVirtualCamera cvc;

    void Awake()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
    }


    public void SetPriority(int newprio)
    {
        cvc.Priority = newprio;
    }
}
