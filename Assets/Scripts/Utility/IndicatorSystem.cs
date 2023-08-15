using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// NOTE: In order to make sure the system works without any transformations of angle in directional indicators (rects, cones)
// MAKE SURE SCALE.Y IS -1 !
// 
// This is because the canvas is laid down on the ground (ground to up), where as the transforms and coordinants of objects in the world are seen looking down (up to ground)
// by making the scale.y -1, you invert the canvas and allow for a 1:1 angle without transforming each incoming angle instruction.

public class IndicatorSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject circularGroundIndicator;
    [SerializeField]
    private GameObject rectangularGroundIndicator;

    public void SpawnCircleAOEIndicator(Vector3 position, Vector2 size, float duration)
    {
        GameObject indicator = Instantiate(circularGroundIndicator, transform);
        indicator.GetComponent<Indicator>().duration = duration;
        RectTransform rt = indicator.GetComponent<RectTransform>();
        rt.anchoredPosition3D = position;
        rt.sizeDelta = size;
    }

    public void SpawnRectangularAOEIndicator(Vector3 startPosition, Vector2 size, float angle, float duration)
    {
        GameObject indicator = Instantiate(rectangularGroundIndicator, transform);
        indicator.GetComponent<Indicator>().duration = duration;
        RectTransform rt = indicator.GetComponent<RectTransform>();
        rt.anchoredPosition3D = startPosition;
        rt.sizeDelta = size;
        rt.rotation = rt.rotation * Quaternion.Euler(0, 0, angle);
    }

    /////////////////// DEBUG ///////////////////////
    public void TestSpawn()
    {
        // SpawnCircleAOEIndicator(position, Vector2.one * 10f, 3f);
    }
    /////////////////// DEBUG ///////////////////////

}
