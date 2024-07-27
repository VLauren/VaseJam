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

        AudioManager.Play("laurent_musica_gameplay", true);
        AudioManager.Stop("laurent_musica_rotura");
        AudioManager.Stop("laurent_musica_menu");
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
        AudioManager.Stop("laurent_musica_gameplay");
        AudioManager.Play("laurent_musica_rotura", true);

        yield return new WaitForSeconds(5);

        while (Time.time < LastScoreTime + 3.5f)
        {
            yield return null;
        }

        currentLevel++;
        if (currentLevel < Levels.Count)
        {
            currentLevelPoints = 0;

            float vol = 0.5f;
            while(vol > 0)
            {
                vol -= Time.deltaTime;
                AudioManager.Play("laurent_musica_rotura", true, vol);
                yield return null;
            }


            AudioManager.Play("laurent_musica_gameplay", true);
            AudioManager.Stop("laurent_musica_rotura");

            SceneManager.LoadScene(Levels[currentLevel]);
        }
        else
        {
            AudioManager.Stop("laurent_musica_gameplay");
            AudioManager.Stop("laurent_musica_rotura");
            AudioManager.Play("laurent_musica_menu", true);

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

    public void DebugNextLevel()
    {
        currentLevel++;
        if (currentLevel < Levels.Count)
        {
            currentLevelPoints = 0;
            SceneManager.LoadScene(Levels[currentLevel]);

            AudioManager.Play("laurent_musica_gameplay", true);
            AudioManager.Stop("laurent_musica_rotura");
        }
        else
        {
            AudioManager.Stop("laurent_musica_gameplay");
            AudioManager.Stop("laurent_musica_rotura");
            AudioManager.Play("laurent_musica_menu", true);

            SceneManager.LoadScene("GameEnd");
        }
    }

    public bool ShouldGoFast()
    {
        return currentLevel > 1;
    }
}
