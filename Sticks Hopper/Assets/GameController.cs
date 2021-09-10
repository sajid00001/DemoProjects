using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//using GameAnalyticsSDK;
//using Facebook.Unity;
public class GameController : MonoBehaviour
{
    public GameObject gamePanel, winPanel, losePanel,infoPanel;

    public GameObject particleEffect;

    string sceneName;

    public TextMeshProUGUI levelName;

    TextMeshProUGUI scoreText;

    int levelIndex = 1;

    public delegate void OnGameStart();
    public static event OnGameStart GameisStarted;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Levels") == 0)
        {
            PlayerPrefs.SetInt("Levels", 1);
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        sceneName = SceneManager.GetActiveScene().name;

        levelIndex = PlayerPrefs.GetInt("Levels");
        levelName.text = "LEVEL : " + levelIndex.ToString();
    }

    private void Start()
    {
        scoreText = winPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        sceneName = SceneManager.GetActiveScene().name;

        TinySauce.OnGameStarted(levelNumber: levelIndex.ToString());

        StartCoroutine(ShowInfo());
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
        levelIndex++;
        PlayerPrefs.SetInt("Levels", levelIndex);
    }

    IEnumerator ShowInfo()
    {
        yield return null;
        if (PlayerPrefs.GetInt("Info") == 0)
        {
            gamePanel.SetActive(false);
            infoPanel.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            infoPanel.SetActive(false);
            gamePanel.SetActive(true);

            GameisStarted();

            PlayerPrefs.SetInt("Info", 100);
        }
        else
        {
            GameisStarted();
        }
    }


    [SerializeField]
    string sceneToLoad;

    [SerializeField]
    float sceneLoadTime;
}
