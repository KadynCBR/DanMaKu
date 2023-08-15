using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CherryTeaGames.Core.Variables;
using CherryTeaGames.Core.Utils;
using DG.Tweening;

public class GlitchControl : MonoBehaviour
{
    public FloatVariable playerHP;
    public Material mat;
    public float noiseAmount;
    public float glitchStrength;
    public float scanlineStrength;
    public float scanlineSpeed;
    public bool performingGlitch;
    public float onHitGlitchIntensity = 30;
    public float glitchDuration = 1f;

    void Update()
    {
        // Scaling with HP
        // mat.SetFloat("_NoiseAmount", Utility.reMap(playerHP.Value, 100, 0, 0, 60));
        mat.SetFloat("_NoiseAmount", noiseAmount);
        mat.SetFloat("_GlitchStrength", glitchStrength);
        mat.SetFloat("_ScanlinesStrength", scanlineStrength);
        mat.SetFloat("_ScanlinesSpeed", scanlineSpeed);
    }

    public void PerformGlitch()
    {
        performingGlitch = true;
        noiseAmount = onHitGlitchIntensity;
        DOVirtual.DelayedCall(glitchDuration, () => { performingGlitch = false; noiseAmount = 0; });
    }
}
