using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailPanel : MonoBehaviour
{
    bool isCalled;

    private void Start()
    {
        isCalled = false;
    }

    private void Update()
    {
        if (!isCalled && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(0);
            isCalled = true;
        }
            
    }
}
