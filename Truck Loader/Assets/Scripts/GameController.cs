using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Facebook.Unity;
using GameAnalyticsSDK;
public class GameController : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;

    public TextMeshProUGUI levelCount;

    [HideInInspector]
    public bool isGameFinished;

    int levelIndex = 1;

    public GameObject winEffect;

    public GameObject winEffect2;

    public GameObject infoPanel;
    public GameObject gamePanel;

    [HideInInspector]
    public bool truckFilled;

    int currencyCount;
    public TextMeshProUGUI currencyText;

    [HideInInspector]
    public int emptyStashCount;
    private void Awake()
    {
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


        truckFilled = false;

        if (PlayerPrefs.GetInt("Levels") == 0)
        {
            PlayerPrefs.SetInt("Levels", 1);
        }

        if (PlayerPrefs.GetInt("Info") == 0)
        {
            StartCoroutine(ShowInfo());
        }

        currencyCount = PlayerPrefs.GetInt("Score", 0);
        currencyText.text = currencyCount.ToString();

        levelIndex = PlayerPrefs.GetInt("Levels");
        levelCount.text = "LEVEL : " + levelIndex.ToString();

        emptyStashCount = 0;

        DroppingObject.ObjectIsLost += RemoveScore;
    }
    
    void Start()
    {
        isGameFinished = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        ObjectCounter.VehicleIsFull += VehicleFull;
        VehicleManage.VehicleIsSwaped += VehicleEmpty;

        GameAnalytics.Initialize();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelIndex.ToString());
    }

    private void OnDisable()
    {
        ObjectCounter.VehicleIsFull -= VehicleFull;
        VehicleManage.VehicleIsSwaped -= VehicleEmpty;
    }

    public void LevelFinished(int score)
    {
        gamePanel.SetActive(false);
        winPanel.SetActive(true);

        AudioEffectsPlayer.PlayAudioClip(5);

        LevelCleared();
        Invoke(nameof(RestartGame), 1.5f);
        isGameFinished = true;

        int rnd = Random.Range(1, 3);
        if (rnd == 1)
            winEffect.SetActive(true);
        else
            winEffect2.SetActive(true);

        AddScore(Random.Range(100, 167));

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelIndex.ToString(), score);
    }


    public void AddScore(int amount)
    {
        StartCoroutine(IncreaseScore(amount));
    }

    void RemoveScore()
    {
        currencyCount--;
        currencyText.text = currencyCount.ToString() + "<color=red>" + " - 1" + "</color>";
        Invoke(nameof(SetCurrency),1f);
    }

    void SetCurrency()
    {
        currencyText.text = currencyCount.ToString();
    }
    IEnumerator IncreaseScore(int amount)
    {
        yield return null;
        int i = 0;
        while (i <= amount)
        {
            currencyCount++;
            currencyText.text = currencyCount.ToString();
            i++;

            yield return new WaitForSeconds(0.01f);         
        }

        PlayerPrefs.SetInt("Score", currencyCount + amount);
    }

    void MoveTruck()
    {
        ObjectCounter.SwapVehicleNow();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void LevelFailed()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelIndex.ToString(), 0);

        isGameFinished = true;
        gamePanel.SetActive(false);
        losePanel.SetActive(true);
    }

    void LevelCleared()
    {
        levelIndex++;
        PlayerPrefs.SetInt("Levels", levelIndex);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator ShowInfo()
    {
        gamePanel.SetActive(false);
        infoPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        infoPanel.SetActive(false);
        gamePanel.SetActive(true);

        PlayerPrefs.SetInt("Info", 99);
    }

    void VehicleFull()
    {
        truckFilled = true;
    }

    void VehicleEmpty()
    {
        truckFilled = false;
    }

    public void RecordEmptyStash()
    {
        emptyStashCount++;

        if (emptyStashCount == 3)
        {
            LevelFinished(000);
        }
    }
}
