using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndTxt : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = "Puntuación:\n" + Game.Instance.totalPoints;
    }

    void OnAny()
    {
        AudioManager.Stop("laurent_musica_gameplay");
        AudioManager.Stop("laurent_musica_rotura");
        AudioManager.Stop("laurent_musica_menu");

        SceneManager.LoadScene(0);
    }
}
