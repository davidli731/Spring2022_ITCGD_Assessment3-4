using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class HUDAspect : MonoBehaviour
{
    private const string hudTag = "HUD";
    private const string scoreTag = "HUDScore";
    private const string scoreText = "Score: ";

    // set this to 4_3 or 16_9 to change aspect ratio
    private string defaultAspectRatio = "16_9";

    private GameObject hudGO;
    public TextMeshProUGUI scoreTMP;

    private static int scoreValue = 0;

    [SerializeField] private GameObject hud4_3;
    [SerializeField] private GameObject hud16_9;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(hudTag);

        // Remove existing canvas
        foreach(GameObject dupe in gameObjects)
        {
            Destroy(dupe);
        }

        // Add new canvas
        if (defaultAspectRatio == "4_3")
        {
            hudGO = Instantiate(hud4_3);
        }
        else if (defaultAspectRatio == "16_9")
        {
            hudGO = Instantiate(hud16_9);
        }

        gameObjects = GameObject.FindGameObjectsWithTag(scoreTag);
        foreach (GameObject go in gameObjects)
        {
            scoreTMP = go.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        scoreTMP.text = scoreText + scoreValue;
    }

    /// <summary>
    /// Add points to score
    /// </summary>
    /// <param name="point"></param>
    public static void AddPoints(Points point)
    {
        scoreValue += (int)point;
    }
}
