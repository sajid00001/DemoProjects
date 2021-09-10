using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FluidSpawner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameController gameController;

    public GameObject valveHandle;

    float originalPosition;
    private void Start()
    {
        isPressed = false;
        originalPosition = valveHandle.transform.localPosition.x;
    }

    bool isPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!gameController.truckFilled)
        {
            if (!gameController.isGameFinished && !isPressed)
            {
                StartCoroutine(OperateSwitch());
            }
        }
    }

    IEnumerator OperateSwitch()
    {
        if (AudioEffectsPlayer.PlayAudioClip != null)
            AudioEffectsPlayer.PlayAudioClip(3);

        LeanTween.moveLocalX(valveHandle, originalPosition + 1.9f, 0.15f);
        yield return new WaitForSeconds(0.5f);
        LeanTween.moveLocalX(valveHandle, originalPosition, 0.5f);
        yield return new WaitForSeconds(0.5f);
        isPressed = false;

        yield return new WaitForSeconds(2f);

        CheckStash();        
    }

    bool stashRecorded = false;

    void CheckStash()
    {
        if (!stashRecorded)
        {
            Collider[] objectColliders = Physics.OverlapBox(transform.position, Vector3.one * 0.4f);

            for (int i = 0; i < objectColliders.Length; i++)
            {
                if (objectColliders[i].gameObject.CompareTag("Object") && objectColliders[i].gameObject.activeInHierarchy)
                {
                    return;
                }
            }
            gameController.RecordEmptyStash();

            stashRecorded = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
