using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGreet : MonoBehaviour
{
    public string[] greetTexts;

    public Color[] textColors;

    public TextMeshProUGUI greetText;

    GameObject textObject;
    void Start()
    {
        textObject = greetText.gameObject;

        textObject.transform.localScale = Vector3.zero;
    }

    public void GreetPlayer()
    {
        StartCoroutine(Greet());
    }

    IEnumerator Greet()
    {
        yield return null;

        if (textObject.transform.localScale != Vector3.zero)
            textObject.transform.localScale = Vector3.zero;

        int randomText = Random.Range(0, greetTexts.Length);
        int randomColor = Random.Range(0, textColors.Length);

        greetText.text = greetTexts[randomText] + "!";
        greetText.color = textColors[randomColor];

        LeanTween.scale(textObject, Vector3.one, 0.15f);

        yield return new WaitForSeconds(1f);

        textObject.transform.localScale = Vector3.zero;
        greetText.text = "";
    }
}
