using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject ennemy;
    public float radius = 10f;

    Coroutine coroutine;

    public void StartSpawning(float delay) {
        StopSpawning();
        coroutine = StartCoroutine(SpawnEnnemy(delay));
    }

    public void StopSpawning() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
    }

    IEnumerator SpawnEnnemy(float delay) {
        while (true) {

            yield return new WaitForSeconds(delay);

            // Spawn the ennemy randomly around the spawn origin, but not in view of main camera
            var go = Instantiate(ennemy);
            do {
                var angle = Random.Range(0f, 2 * Mathf.PI);
                var spawnPosition = transform.position;
                spawnPosition.x += radius * Mathf.Cos(angle);
                spawnPosition.z += radius * Mathf.Sin(angle);
                go.transform.position = spawnPosition;

                // Object should face the camera
                go.transform.forward = (spawnPosition - transform.position);

            } while (Utils.IsSeenByMainCamera(go.GetComponentInChildren<MeshRenderer>()));

        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawMesh(ennemy.GetComponent<Mesh>(), transform.position);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
