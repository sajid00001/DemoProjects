using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Vector3 movementDir;

    Vector3 initialmousePos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        initialmousePos = Input.mousePosition;
        //initialmousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 finalPos = (Input.mousePosition);

        movementDir = finalPos - initialmousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        movementDir = Vector3.zero;
    }

}
