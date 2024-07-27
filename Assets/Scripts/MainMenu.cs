using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        AudioManager.Stop("laurent_musica_gameplay");
        AudioManager.Stop("laurent_musica_rotura");
        AudioManager.Play("laurent_musica_menu", true);
    }

    void OnAny(InputValue value)
    {
        Game.Instance.StartGame();
    }
}
