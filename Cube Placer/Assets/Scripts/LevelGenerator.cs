using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    GameObject player;

    public GameObject cube;

    public List<Material> cubeMat;

    [SerializeField]
    int cubesCount;

    float height = 1f;

    public List<GameObject> cubes;

    int previousMat;
    int rnd;
    void Start()
    {
        player = transform.GetChild(0).transform.GetChild(0).gameObject;

        for (int i = 0; i < cubesCount; i++)
        {
            GameObject tmp = Instantiate(cube);

            while (rnd == previousMat)
            {
                rnd = Random.Range(0, cubeMat.Count);
            }
            
            tmp.GetComponent<Renderer>().material = cubeMat[rnd];
            previousMat = rnd;

            tmp.transform.parent = gameObject.transform.GetChild(0).transform;
            tmp.transform.localPosition = Vector3.zero;

            tmp.GetComponent<Rigidbody>().isKinematic = true;

            tmp.transform.position += new Vector3(0f, height, 0f);

            cubes.Add(tmp);

            height++;
        }

        player.transform.position += new Vector3(0f, (height - 1) + 0.52f, 0f);

        SetPlayerMaterial();

    }

    [SerializeField]
    GameObject character;
    void SetPlayerMaterial()
    {
        int rnd = Random.Range(0, cubeMat.Count);
        character.GetComponent<Renderer>().material = cubeMat[rnd];
    }
}
