using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FinishLine : MonoBehaviour
{
    public Image progressBarFillImage;

    [SerializeField]
    Transform playerTransfrom;

    float maxDistance;

    void Start()
    {
        maxDistance = Vector3.Distance(playerTransfrom.position, transform.position);
    }

    void FixedUpdate()
    {
        float fill = Vector3.Distance(playerTransfrom.position, transform.position);


        progressBarFillImage.fillAmount = (fill/maxDistance) ;
    }

}
