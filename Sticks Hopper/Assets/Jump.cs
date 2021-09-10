using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pole"))
        {
            GameObject tmp = other.transform.parent.gameObject;

            Debug.Log("Fired!");
        }
    }
}
