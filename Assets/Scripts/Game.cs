using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public List<string> Levels;

    int currentLevel = 0;
    int currentLevelPoints;
    int totalPoints;

    float LastScoreTime;

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
        LastScoreTime = Time.time;
        print("score += " + _score + " - total: " + currentLevelPoints);
    }

    public void LevelEnd()
    {
        StartCoroutine(LevelEndRoutine());
    }

    IEnumerator LevelEndRoutine()
    {
        yield return new WaitForSeconds(10);


        while (Time.time < LastScoreTime + 4)
        {
            print(Time.time + " < " + (LastScoreTime + 7));
            yield return null;
        }

        currentLevel++;
        if (currentLevel < Levels.Count)
        {
            totalPoints += currentLevelPoints;
            currentLevelPoints = 0;
            SceneManager.LoadScene(Levels[currentLevel]);
        }
        else
        {
            // Game End
            print("GAME END!!");
            SceneManager.LoadScene(0);

        }
    }

}
