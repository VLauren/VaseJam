using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = Game.Instance.GetLevelText();
    }
}
