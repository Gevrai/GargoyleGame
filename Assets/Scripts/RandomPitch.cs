using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomPitch : MonoBehaviour
{
    [Range(0,0.5f)]
    public float Ratio;
    
    void Start() {
        GetComponent<AudioSource>().pitch *= Random.Range(1 - Ratio, 1 + Ratio);
    }
}
