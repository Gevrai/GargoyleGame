using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTooClose : MonoBehaviour
{
    [SerializeField] float distance;

    void Update() {
        var d = Vector3.Distance(Camera.main.transform.position, gameObject.transform.position);
        if (d < distance) {
            GameManager.Instance.OnGargoyleTooClose(gameObject);
            enabled = false;
        }
    }
}
