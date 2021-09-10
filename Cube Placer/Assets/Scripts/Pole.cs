using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    PlayerController playerController;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.parent != null)
            playerController = collision.transform.parent.gameObject.GetComponent<PlayerController>();

        if (playerController != null)
            playerController.HitPole();
    }
}
