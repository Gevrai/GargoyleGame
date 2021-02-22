using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct DifficultySetting {
    public string Name;
    public float SpawnRate;
}

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject startMenuUI;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject gameOverUI;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Spawner spawner;

    [SerializeField] AudioClip roarSound;

    [SerializeField] DifficultySetting[] difficultySettings;

    int score = 0;

    internal void AddToScore(int delta) {
        score += delta;
        scoreText.text = "Score: " + score.ToString();
    }

    void Awake() { GameManager.Instance = this; }

    void Start() {
        startMenuUI.SetActive(true);
        inGameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<MouseLook>().enabled = false;
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }

    public void StartWithDifficulty(string difficulty) {
        float spawnRate = 0f;
        foreach (var setting in difficultySettings) {
            if (setting.Name == difficulty) {
                spawnRate = setting.SpawnRate;
                break;
            }
        }
        if (spawnRate == 0f) {
            Debug.LogError("Couldn't find difficulty level " + difficulty + ". Defaulting to 1 spawn per second.");
            spawnRate = 1f;
        }

        AddToScore(0);
        Cursor.lockState = CursorLockMode.Locked;

        startMenuUI.SetActive(false);
        inGameUI.SetActive(true);

        // Show the player how to kill the gargoyles
        var gargoyle = FindObjectOfType<FelineGargoyle>();
        var directionToLook = gargoyle.leftEye.transform.position - Camera.main.transform.position;
        StartCoroutine(Utils.LookAtSmooth(Camera.main.gameObject, directionToLook, 0.7f));
        StartCoroutine(Utils.RunAfterDelay(1f, () => {
            Destroy(gargoyle.gameObject);
            Camera.main.GetComponent<MouseLook>().enabled = true;
            spawner.StartSpawning(spawnRate);
        }));
    }

    public void OnGargoyleTooClose(GameObject gargoyle) {
        Debug.Log("game over!");

        // Disable input and gargoyle movement
        Camera.main.GetComponent<MouseLook>().enabled = false;
        inGameUI.SetActive(false);
        spawner.StopSpawning();
        foreach(var move in FindObjectsOfType<MoveIfUnseen>()) {
            move.enabled = false;
        }
        foreach (var audio in FindObjectsOfType<AudioSource>()) {
            audio.Stop();
        }

        // Quickly look at the KILLER
        var eye = gargoyle.GetComponent<FelineGargoyle>().leftEye;
        var directionToLook = eye.transform.position - Camera.main.transform.position;
        StartCoroutine(Utils.LookAtSmooth(Camera.main.gameObject, directionToLook, 0.2f));

        // ROAR
        var roar = gargoyle.AddComponent<AudioSource>();
        roar.PlayOneShot(roarSound);

        // Show game over screen after delay
        StartCoroutine(Utils.RunAfterDelay(2f, () => {
            gameOverUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }));
    }
}
