using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    [SerializeField] int score;

    public int Kill() {
        Destroy(gameObject);
        return score;
    }
}
