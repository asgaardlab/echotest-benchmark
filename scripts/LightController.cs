using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightController : MonoBehaviour
{
    private Light _light;
    [Range(0.0f, 1.0f)] readonly float maxIntensity = 1.0f;
    [Range(0.0f, 1.0f)] readonly float minIntensity = 0.5f;
    private void Start()
    {
        _light = GetComponent<Light>();
    }

    public IEnumerator DimLight(float duration)
    {
        var elapsedTime = 0.0f;
        var amplitude = maxIntensity - minIntensity;
        var x = 0f;
        var angularVelocity = (float) Math.PI * Time.deltaTime;
        while (elapsedTime < duration)
        {
            _light.intensity = (float) Math.Abs(amplitude * Math.Cos(x)) + minIntensity;
            elapsedTime += Time.deltaTime;
            x += angularVelocity;
            yield return new WaitForSeconds(elapsedTime);
        }
    }
}
