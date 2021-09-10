using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHand : MonoBehaviour
{
    public GameObject hand;

    bool isShown = false;

    RectTransform handRect;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isShown == false)
        {
            StartCoroutine(DisplayHand());
            isShown = true;
        }
    }

    [SerializeField]
    float handDisplayTime;
    IEnumerator DisplayHand()
    {
        handRect = hand.GetComponent<RectTransform>();
        handRect.position = Input.mousePosition;
        hand.SetActive(true);
        yield return new WaitForSeconds(handDisplayTime);
        hand.SetActive(false);

        isShown = false;
    }
}
