using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CherryTeaGames.Core.Variables;

public class VariableSlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private FloatVariable variable;


    void FixedUpdate()
    {
        slider.value = Mathf.Lerp(slider.value, variable.Value, .25f);
    }
}
