using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManage : MonoBehaviour
{
    public GameObject vehicle;

    public Material[] colors;

    [SerializeField]
    List<GameObject> tyres;

    float originalPosition;
    [SerializeField]
    GameObject truckBody;

    public delegate void VehicleShifted();
    public static event VehicleShifted VehicleIsSwaped;

    public delegate void UnloadVehicle();
    public static event UnloadVehicle UnloadVehicleNow;

    private void Start()
    {
        originalPosition = vehicle.transform.position.x;

        ObjectCounter.SwapVehicleNow += ShiftVehicle;

        ChangePaint();
    }

    private void OnDisable()
    {
        ObjectCounter.SwapVehicleNow -= ShiftVehicle;
    }


    void ShiftVehicle()
    {
        StartCoroutine(SwapVehicle());
    }

    void RotateTyres()
    {
        for (int i = 0; i < tyres.Count; i++)
        {
            LeanTween.rotateX(tyres[i], -3600f, 3f);
        }
    }

    void ChangePaint()
    {
        if (truckBody != null)
        {
            int rnd = Random.Range(0, colors.Length);
            truckBody.GetComponent<Renderer>().material = colors[rnd];
        }
    }


    IEnumerator SwapVehicle()
    {
        LeanTween.moveX(vehicle, originalPosition * 5f, 2f);

        RotateTyres();

        yield return new WaitForSeconds(2f);

        ChangePaint();

        UnloadVehicleNow();

        vehicle.transform.position = new Vector3(originalPosition - 25f, vehicle.transform.position.y, vehicle.transform.position.z);

        LeanTween.moveX(vehicle, originalPosition , 1f);

        yield return new WaitForSeconds(1f);

        AudioEffectsPlayer.PlayAudioClip(2);

        VehicleIsSwaped();
    }
}
