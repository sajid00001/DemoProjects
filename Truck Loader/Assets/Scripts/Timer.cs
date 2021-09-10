using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    [SerializeField]
    float timer;

    bool eventFired;

    public GameController gameController;

    private void Start()
    {
        eventFired = false;
    }

    void Update()
    {
        if (!eventFired)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                string minutes = Mathf.Floor(timer / 60).ToString("00");
                string seconds = (timer % 60).ToString("00");
                timerText.text = minutes + ":" + seconds;
            }
            else
            {
                if (!eventFired)
                {
                    gameController.LevelFailed();
                    AudioEffectsPlayer.PlayAudioClip(4);
                    timerText.text = "00:00";
                    eventFired = true;
                }
            }
        }
    }
}
