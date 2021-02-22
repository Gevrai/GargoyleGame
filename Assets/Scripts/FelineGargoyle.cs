using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used only for referencing
public class FelineGargoyle : MonoBehaviour {

    public GameObject leftEye;
    private void Start() {
        leftEye = transform.Find("LeftEye").gameObject;
    }

}
