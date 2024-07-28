using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextoPuntosNivel : MonoBehaviour
{
    TMP_Text Text;

    int ScoreToShow = 0;

    void Start()
    {
        Text = GetComponent<TMP_Text>();
        Text.enabled = false;
    }

    void Update()
    {
        if(Game.Instance.currentLevelPoints > 0)
        {
            Text.enabled = true;
            if(ScoreToShow < Game.Instance.currentLevelPoints)
            {
                if (Game.Instance.currentLevelPoints - ScoreToShow > 40000)
                    ScoreToShow += 40;
                else
                    ScoreToShow += 300;

                if (ScoreToShow > Game.Instance.currentLevelPoints)
                    ScoreToShow = Game.Instance.currentLevelPoints;

                Game.Instance.LastScoreTime = Time.time;
            }
            Text.text = "+" + ScoreToShow;
        }
    }
}
