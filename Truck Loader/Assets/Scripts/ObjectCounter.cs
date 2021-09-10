using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ObjectCounter : MonoBehaviour
{
    [SerializeField]
    int totalCount;

    public int maxCount;

    public delegate void VehicleIsFilled();
    public static event VehicleIsFilled VehicleIsFull;

    public delegate void SwapVehicle();
    public static SwapVehicle SwapVehicleNow;

    Button nextTruckButton;

    public List<GameObject> fallenObject;

    float positionDiff;

    public GameController gameController;

    [SerializeField]
    int truckTotalTurns;

    public GameObject loadChecker;
    private void Awake()
    {
        DroppingObject.ObjectIsFallen += Count;
    }
    private void Start()
    {
        VehicleManage.VehicleIsSwaped += OnVehicleShift;
        VehicleManage.UnloadVehicleNow += Unload;

        //objectCountText.text = totalCount.ToString() + " / " + maxCount.ToString();

        objectCountText.text = truckTotalTurns.ToString() + " Trucks\nRemaining";

        if (PlayerPrefs.GetInt("Levels", 0) < 3)
        {
            truckTotalTurns++;
            objectCountText.text = truckTotalTurns.ToString() + " Trucks\nRemaining";
        }           
    }

    private void OnDisable()
    {
        VehicleManage.VehicleIsSwaped -= OnVehicleShift;
        VehicleManage.UnloadVehicleNow -= Unload;
        DroppingObject.ObjectIsFallen -= Count;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            tmp.transform.position = new Vector3(transform.position.x - positionDiff, tmp.transform.position.y, tmp.transform.position.z);
        }
    }

    GameObject tmp;

    public TextMeshProUGUI objectCountText;

    /*
    void Count(GameObject tmp)
    {
        if (tmp != null)
        {
            fallenObject.Add(tmp);

            totalCount++;

            //objectCountText.text = totalCount.ToString() + "/" + maxCount.ToString();

            if (totalCount == maxCount)
            {
                nextButton.SetActive(true);

                VehicleIsFull();

                nextTruckButton.onClick.AddListener(OnButtonClick);

                AudioEffectsPlayer.PlayAudioClip(0);
            }
        }
    }
    */

    void Count(GameObject tmp)
    {
        if (tmp != null)
        {
            fallenObject.Add(tmp);

            totalCount++;
        }
    }

    public void OnButtonClick()
    {
        AudioEffectsPlayer.PlayAudioClip(0);

        tmp = new GameObject("Temp");
        positionDiff = transform.position.x - tmp.transform.position.x;

        AudioEffectsPlayer.PlayAudioClip(1);

        for (int i = 0; i < fallenObject.Count; i++)
        {
            fallenObject[i].GetComponent<Rigidbody>().isKinematic = true;
            fallenObject[i].transform.parent = tmp.transform;
        }

        isMoving = true;

        gameController.AddScore(totalCount);

        SwapVehicleNow();

        totalCount = 0;

        if (truckTotalTurns >= 1)
        {
            truckTotalTurns--;
            objectCountText.text = truckTotalTurns.ToString() + " Trucks\nRemaining";
        }

        if (truckTotalTurns == 0 && gameController.emptyStashCount < 3)
            gameController.LevelFailed();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            Handheld.Vibrate();
    }

    bool isMoving = false;

    void OnVehicleShift()
    {
        //objectCountText.text = totalCount.ToString() + "/" + maxCount.ToString();
    }

    void Unload()
    {
        for (int i = 0; i < fallenObject.Count; i++)
        {
            fallenObject[i].GetComponent<Rigidbody>().isKinematic = false;
        }

        isMoving = false;

        fallenObject.Clear();
    }
}
