using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using GameAnalyticsSDK;
using Facebook.Unity;
public class GameController : MonoBehaviour
{
    public GameObject gamePanel, winPanel, losePanel;

    public GameObject particleEffect;

    bool gameOver,gameWin;

    string sceneName;

    public Text levelName;

    public TextMeshProUGUI scoreText;

    int levelIndex = 1;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Levels") == 0)
        {
            PlayerPrefs.SetInt("Levels", 1);
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        gameOver = false;
        gameWin = false;

        sceneName = SceneManager.GetActiveScene().name;


        levelIndex = PlayerPrefs.GetInt("Levels");
        levelName.text = levelIndex.ToString();
        //levelName.color = Color.black;

        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
            });
        }

    }

    private void Start()
    {
        GameAnalytics.Initialize();

        gameOver = false;
        gameWin = false;

        sceneName = SceneManager.GetActiveScene().name;

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelIndex.ToString());
    }

    public void LevelFailed()
    {
        gamePanel.SetActive(false);
        losePanel.SetActive(true);

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelIndex.ToString(), 0);

        StartCoroutine(LoadScene(sceneName));
    }

    public void LevelFinished(int scoreCount)
    {
        gamePanel.SetActive(false);
        winPanel.SetActive(true);

        LevelCleared();

        gameWin = true;

        particleEffect.SetActive(true);

        StartCoroutine(LoadScene(sceneToLoad));

        scoreText.text = "SCORE : " + scoreCount.ToString();

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelIndex.ToString(), Mathf.RoundToInt(scoreCount));
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
