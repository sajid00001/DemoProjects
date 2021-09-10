using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Material referenceMat;

    public int levelIndex;

    public Color[] colors;

    public delegate void LevelGenerated();
    public static event LevelGenerated LevelisGenerated;

    int colorIndex;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Color") > 34)
        {
            PlayerPrefs.SetInt("Color", 0);
        }

        colorIndex = PlayerPrefs.GetInt("Color", 0);
        referenceMat.color = Color.white;
        GenerateLevel();
    }

    void GenerateLevel()
    {
        colors[colorIndex].a = 1f;
        referenceMat.color = colors[colorIndex];

        LevelisGenerated();

        colorIndex++;
        PlayerPrefs.SetInt("Color", colorIndex);
    }
}
