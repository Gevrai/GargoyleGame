using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Flicker : MonoBehaviour {
    [Range(0, 1)] public float intensityDeltaRatio = 0.1f;

    public float speed = 10f;

    private Light light;
    private float initialIntensity;

    void Start() {
        light = GetComponent<Light>();
        initialIntensity = light.intensity;
    }

    void Update() {
        // Smooth flickering of light intensity
        var rand = Mathf.PerlinNoise(Time.deltaTime * speed, 0);
        var delta = initialIntensity * intensityDeltaRatio;
        light.intensity = Mathf.Lerp(initialIntensity - delta, initialIntensity + delta, rand);
    }
}
