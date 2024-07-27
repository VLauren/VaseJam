using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndTxt : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = "Puntuaci�n:\n" + Game.Instance.totalPoints;
    }

    void OnAny()
    {
        Game.Instance.StartGame();
    }
}
