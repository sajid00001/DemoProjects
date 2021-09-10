using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPositionsChecker : MonoBehaviour
{
    public HolesRecord holesRecorder;

    public List<Vector3> holePositions;

    private void Start()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Vector3 pos = gameObject.transform.GetChild(i).gameObject.transform.position;
            holePositions.Add(pos);
        }

        holesRecorder.RecordPositions(holePositions);
    }
}
