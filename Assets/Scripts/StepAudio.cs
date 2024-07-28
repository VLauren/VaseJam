using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepAudio : MonoBehaviour
{
    void Step()
    {
        int index = Random.Range(1, 6);
        AudioManager.Play("paso" + index, false, 0.35f);
    }
}
