using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public List<string> Levels;

    int currentLevel = 0;
    public int currentLevelPoints;
    public int totalPoints;

    public float LastScoreTime;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        totalPoints = 0;
        currentLevelPoints = 0;
        currentLevel = 0;

        SceneManager.LoadScene(Levels[0]);
    }

    public void AddScore(int _score)
    {
        currentLevelPoints += _score;
        totalPoints += _score;
        LastScoreTime = Time.time;
    }

    public void LevelEnd()
    {
        StartCoroutine(LevelEndRoutine());
    }

    IEnumerator LevelEndRoutine()
    {
        yield return new WaitForSeconds(3);

        while (Time.time < LastScoreTime + 3.5f)
        {
            yield return null;
        }

        currentLevel++;
        if (currentLevel < Levels.Count)
        {
            // totalPoints += currentLevelPoints;
            currentLevelPoints = 0;
            SceneManager.LoadScene(Levels[currentLevel]);
        }
        else
        {
            // Game End
            SceneManager.LoadScene("GameEnd");
        }
    }

    public void RepeatLevel()
    {
        StartCoroutine(RepeatLevelRoutine());
    }

    IEnumerator RepeatLevelRoutine()
    {
        yield return new WaitForSeconds(1);

        var txt = FindObjectOfType<TextoPuntosNivel>().GetComponent<TMP_Text>();
        txt.enabled = true;
        txt.text = "+0 puntos";

        yield return new WaitForSeconds(4);

        currentLevelPoints = 0;
        SceneManager.LoadScene(Levels[currentLevel]);
    }

}
