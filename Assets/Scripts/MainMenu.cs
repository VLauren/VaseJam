using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void OnAny(InputValue value)
    {
        Game.Instance.StartGame();
    }
}
