using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{

    void OnAny()
    {
        Game.Instance.StartGame();
    }
}
