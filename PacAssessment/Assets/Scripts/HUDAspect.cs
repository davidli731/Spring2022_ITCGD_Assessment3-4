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
    private const string ghostTimerTag = "HUDScaredTimer";
    private const string ghostTimerText = "Scared Timer: ";

    // set this to 4_3 or 16_9 to change aspect ratio
    private string defaultAspectRatio = "16_9";

    private GameObject hudGO;
    private GameObject[] LivesGO;
    private TextMeshProUGUI scoreTMP;
    private TextMeshProUGUI GhostScaredTimerTMP;

    private static int scoreValue = 0;

    [SerializeField] private GameObject hud4_3;
    [SerializeField] private GameObject hud16_9;

    public static bool IsTimerActive = false;
    public static int GhostTimerValue = 0;
    public static int LifeCount = 3;

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
        foreach(GameObject go in gameObjects)
        {
            scoreTMP = go.GetComponentInChildren<TextMeshProUGUI>();
        }

        gameObjects = GameObject.FindGameObjectsWithTag(ghostTimerTag);
        foreach (GameObject go in gameObjects)
        {
            GhostScaredTimerTMP = go.GetComponentInChildren<TextMeshProUGUI>();
        }

        LivesGO = new GameObject[LifeCount];
        for (int i = 1; i <= 3; i++)
        {
            gameObjects = GameObject.FindGameObjectsWithTag("Live" + i);
            foreach(GameObject go in gameObjects)
            {
                LivesGO[i - 1] = go;
            }
        }
    }

    private void Update()
    {
        scoreTMP.text = scoreText + scoreValue;

        if (IsTimerActive)
        {
            GhostScaredTimerTMP.gameObject.SetActive(true);
            GhostScaredTimerTMP.text = ghostTimerText + GhostTimerValue;
        }
        else
        {
            GhostScaredTimerTMP.gameObject.SetActive(false);
        }

        if (LifeCount <= 2)
        {
            checkAndUpdateLife(2);

            if (LifeCount <= 1)
            {
                checkAndUpdateLife(1);

                if (LifeCount == 0)
                {
                    checkAndUpdateLife(0);
                }
            }
        }
    }

    private void checkAndUpdateLife(int i)
    {
        if (LivesGO[i].activeSelf)
        {
            LivesGO[i].SetActive(false);
        }
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
