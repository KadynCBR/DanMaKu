using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinoidBoss : MonoBehaviour
{
    [SerializeField]
    private Transform baseTransform;
    private IndicatorSystem indicatorSystem;


    void Awake()
    {
        indicatorSystem = GameObject.Find("GroundIndicatorCanvas").GetComponent<IndicatorSystem>();
    }

    public void DropAOE()
    {
        indicatorSystem.SpawnCircleAOEIndicator(
            new Vector3(baseTransform.position.x, indicatorSystem.transform.position.y, baseTransform.position.z),
            Vector2.one * 10f,
            3f
        );
    }

    private float Timer;
    void FixedUpdate()
    {
        baseTransform.Translate(Vector3.forward * Time.deltaTime * 3, Space.Self);
        Timer += Time.deltaTime;
        if (Timer > 3f)
        {
            Timer = 0;
            DropAOE();
        }
    }
}
