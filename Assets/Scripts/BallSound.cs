using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSound : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        int index = Random.Range(1, 4);
        AudioManager.Play("pelota" + index, false, 0.35f);
    }
}
