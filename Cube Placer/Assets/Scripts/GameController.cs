using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject gamePanel, winPanel, losePanel;

    public GameObject particleEffect;

    string sceneName;

    public TextMeshProUGUI levelName,nextLevelName;

    public TextMeshProUGUI scoreText;

    int levelIndex = 1;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Levels") == 0)
        {
            PlayerPrefs.SetInt("Levels", 1);
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        sceneName = SceneManager.GetActiveScene().name;


        levelIndex = PlayerPrefs.GetInt("Levels");
        levelName.text = levelIndex.ToString();
        nextLevelName.text = (levelIndex + 1).ToString();
        
    }

    private void Start()
    {  
        sceneName = SceneManager.GetActiveScene().name;

        TinySauce.OnGameStarted(levelNumber: levelIndex.ToString());
    }

    public void LevelFailed()
    {
        gamePanel.SetActive(false);
        losePanel.SetActive(true);

        TinySauce.OnGameFinished(false, 0, levelNumber: levelIndex.ToString());

        StartCoroutine(LoadScene(sceneName));
    }

    public void LevelFinished(int scoreCount)
    {
        gamePanel.SetActive(false);
        winPanel.SetActive(true);

        LevelCleared();

        particleEffect.SetActive(true);

        StartCoroutine(LoadScene(sceneToLoad));

        scoreText.text = "SCORE : " + scoreCount.ToString();

        TinySauce.OnGameFinished(true, scoreCount, levelNumber: levelIndex.ToString());
    }

    IEnumerator LoadScene(string sceneName)
    {
        if (sceneName == sceneToLoad)
            yield return new WaitForSeconds(sceneLoadTime + 0.25f);
        else
            yield return new WaitForSeconds(sceneLoadTime);

        SceneManager.LoadScene(sceneName);
    }

    void LevelCleared()
    {
        Debug.Log(levelIndex);
        levelIndex++;
        Debug.Log(levelIndex);
        PlayerPrefs.SetInt("Levels", levelIndex);
    }


    [SerializeField]
    string sceneToLoad;

    [SerializeField]
    float sceneLoadTime;
}
