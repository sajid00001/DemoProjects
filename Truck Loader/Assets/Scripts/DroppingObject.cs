using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingObject : MonoBehaviour
{
    Rigidbody physicsBody;

    public delegate void ObjectSpawned(GameObject obj);
    public static event ObjectSpawned ObjectIsFallen;

    public delegate void ObjectLost();
    public static event ObjectLost ObjectIsLost;
    private void Start()
    {
        physicsBody = GetComponent<Rigidbody>();
    }

    bool executed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Counter") && !executed)
        {
            if (ObjectIsFallen != null && gameObject != null)
                ObjectIsFallen?.Invoke(gameObject);

            transform.parent = null;
            gameObject.layer = 8;

            executed = true;
        }
        else if (other.gameObject.CompareTag("Lost"))
        {
            if (ObjectIsLost != null)
                ObjectIsLost();
        }

        else if (other.gameObject.CompareTag("Boundary"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        physicsBody.velocity = Vector3.zero;
    }
}
