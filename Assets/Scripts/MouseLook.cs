using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

    private static Vector3 centerScreen = new Vector3(0.5f, 0.5f, 0);
    public float mouseSensitivity = 100f;

    private void Start() {
#if UNITY_WEBGL
        // Weird issue where sensitivity and acceleration are very different on WebGL builds...
        Debug.Log("WebGL build detected");
        mouseSensitivity /= 4;
#endif
    }

    // Update is called once per frame
    void Update() {
        var dx = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        var dy = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var curr = transform.localRotation.eulerAngles;

        var x = curr.x - dx;
        x = x > 180 ? x - 360: x;
        x = Mathf.Clamp(x, -80f, 80f);

        var y = curr.y + dy;

        transform.localRotation = Quaternion.Euler(x, y, 0);

        if (Input.GetMouseButtonDown(0)) {
            Shoot();
        }
    }

    void Shoot() {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ViewportPointToRay(centerScreen), out hit)) {
            return;
        }
        var killable = Utils.GetComponenInObjectOrParent<Killable>(hit.collider.gameObject);
        if (killable) {
            var score = killable.Kill();
            GameManager.Instance.AddToScore(score);
        }
    }
}