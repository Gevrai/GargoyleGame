using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIfUnseen : MonoBehaviour {
    public float speed = 2f;
    public float gameOverDistance = 2f;

    private Transform destination;
    private Renderer renderer;
    private AudioSource audioSource;

    void Start() {
        renderer = GetComponentInChildren<MeshRenderer>();
        destination = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        // Move towards camera if player not currently looking at game object
        if (!Utils.IsSeenByMainCamera(renderer)) {
            var direction = (destination.position - transform.position);
            transform.position += direction.normalized * speed * Time.deltaTime;
            audioSource.volume = 1f;
        } else {
            audioSource.volume = 0f;
        }
    }

}
