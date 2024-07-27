using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AlphaTextOsc : MonoBehaviour
{
    public float MinAlpha;
    public float MaxAlpha;
    public float OscFreq;

    void Update()
    {
        Color c = GetComponent<Text>().color;
        c.a = Mathf.Lerp(MinAlpha, MaxAlpha, (Mathf.Cos(OscFreq * Time.time * 2 * Mathf.PI) / 2 + 0.5f));
        GetComponent<Text>().color = c;
    }
}
