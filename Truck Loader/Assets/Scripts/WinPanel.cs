using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinPanel : MonoBehaviour
{
    public TextMeshProUGUI complimentText;
    public TextMeshProUGUI greetText;

    [SerializeField]
    string[] complimentTexts;

    [SerializeField]
    string[] greetTexts;

    [SerializeField]
    Color[] textColors;

    void onEnable()
    {
        int rndColor1 = Random.Range(0, textColors.Length);
        complimentText.color = textColors[rndColor1];

        //int rndColor2 = Random.Range(0, textColors.Length);
        greetText.color = textColors[rndColor1];

        int rndTextComp = Random.Range(0, complimentTexts.Length);
        complimentText.text = complimentTexts[rndTextComp];

        int rndTextGreet = Random.Range(0, greetTexts.Length);
        greetText.text = greetTexts[rndTextGreet];

        LeanTween.scale(gameObject, Vector3.one, 0.5f);
    }
}
