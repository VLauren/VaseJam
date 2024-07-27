using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextoPuntosTotal : MonoBehaviour
{
    TMP_Text Text;

    int ScoreToShow = 0;

    void Start()
    {
        Text = GetComponent<TMP_Text>();
        // Text.enabled = false;
    }

    void Update()
    {
        if(Game.Instance.totalPoints > 0)
        {
            // Text.enabled = true;
            if(ScoreToShow < Game.Instance.totalPoints)
            {
                ScoreToShow += 15;
                if (ScoreToShow > Game.Instance.totalPoints)
                    ScoreToShow = Game.Instance.totalPoints;
            }
            Text.text = "Puntos: " + ScoreToShow;
        }
    }
}
