using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolesRecord : MonoBehaviour
{
    public List<Vector3> holePositions;

    public void RecordPositions(List<Vector3> pos)
    {
        for (int i = 0; i < pos.Count; i++)
        {
            holePositions.Add(pos[i]);
        }
    }
}
