using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class HUDAspect : MonoBehaviour
{
    private const string hudTag = "HUD";

    // set this to 4_3 or 16_9 to change aspect ratio
    private string defaultAspectRatio = "16_9";

    [SerializeField] private GameObject hud4_3;
    [SerializeField] private GameObject hud16_9;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] dupes = GameObject.FindGameObjectsWithTag(hudTag);

        // Remove existing canvas
        foreach(GameObject dupe in dupes)
        {
            Destroy(dupe);
        }

        // Add new canvas
        if (defaultAspectRatio == "4_3")
        {
            Instantiate(hud4_3);
        }
        else if (defaultAspectRatio == "16_9")
        {
            Instantiate(hud16_9);
        }
    }
}
