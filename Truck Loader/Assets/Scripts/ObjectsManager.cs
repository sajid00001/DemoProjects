using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{
    public Material[] materials;

    public List<GameObject> stashes;

    public GameObject[] objects;

    [SerializeField]
    int stackSize;

    [SerializeField]
    int largeObjectCount;

    [SerializeField]
    int mediumObjectCount;

    [SerializeField]
    int smallObjectCount;
    private void Start()
    {
        StartCoroutine(FillStashOne());
        StartCoroutine(FillStashTwo());
        StartCoroutine(FillStashThree());
    }

    // Large Objects Stash
    IEnumerator FillStashOne()
    {        
        while (largeObjectCount > 0)
        {
            GenerateObject(stashes[0].transform,2.5f);
            largeObjectCount--;
            yield return new WaitForSeconds(0.025f);
        }
    }

    // Medium Objects Stash
    IEnumerator FillStashTwo()
    {
        while (mediumObjectCount > 0)
        {
            GenerateObject(stashes[1].transform, 1.5f);
            mediumObjectCount--;
            yield return new WaitForSeconds(0.025f);
        }
    }

    // Small Objects Stash
    IEnumerator FillStashThree()
    {
        while (smallObjectCount > 0)
        {
            GenerateObject(stashes[2].transform, 1f);
            smallObjectCount--;
            yield return new WaitForSeconds(0.025f);
        }
    }

    void GenerateObject(Transform trans,float scale)
    {
        int rnd = Random.Range(0, objects.Length);
        GameObject tmp = Instantiate(objects[rnd], trans.position +
            new Vector3(0f, 7f, Random.Range(-0.5f, 0.5f)), Quaternion.identity);

        int rnd2 = Random.Range(0, materials.Length);
        int rnd3 = Random.Range(0, materials.Length);

        if (tmp.transform.childCount > 1)
        {
            tmp.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = materials[rnd2];
            tmp.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = materials[rnd3];
        }

        tmp.transform.localScale *= scale;

        tmp.transform.parent = trans;
        tmp.SetActive(true);
    }

}
