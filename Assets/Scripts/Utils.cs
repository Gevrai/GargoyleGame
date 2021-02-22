using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Utils {
    public static bool IsSeenByMainCamera(Renderer renderer) {
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    public static T GetComponenInObjectOrParent<T>(GameObject obj) where T : MonoBehaviour {
        var c = obj.GetComponent<T>();
        return c ?? obj.GetComponentInParent<T>();
    }

    public static IEnumerator LookAtSmooth(GameObject objectToRotate, Vector3 finalLookDirection, float delay) {
        float startTime = Time.time;
        float endTime = startTime + delay;

        var startRotation = Quaternion.LookRotation(objectToRotate.transform.forward);
        var endRotation = Quaternion.LookRotation(finalLookDirection.normalized);

        while (Time.time < endTime) {
            var t = Mathf.InverseLerp(startTime, endTime, Time.time);
            objectToRotate.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }
        objectToRotate.transform.forward = finalLookDirection.normalized;
    }

    public static IEnumerator RunAfterDelay(float delay, Action action) {
        yield return new WaitForSeconds(delay);
        action();
    }
}
