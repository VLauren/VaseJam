using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceIndicator : MonoBehaviour
{
    void Update()
    {
        if (MainChar.Instance.CanControl)
            GetComponent<Slider>().value = -MainChar.Instance.GetVaseTilt() / 2 + 0.5f;
    }
}
