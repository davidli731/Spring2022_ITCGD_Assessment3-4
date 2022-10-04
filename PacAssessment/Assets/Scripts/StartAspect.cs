using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class StartAspect : MonoBehaviour
{
    private const string startCanvasTag = "StartCanvas";

    // set this to 4_3 or 16_9 to change aspect ratio
    private string defaultAspectRatio = "16_9";

    [SerializeField] private GameObject startCanvas4_3;
    [SerializeField] private GameObject startCanvas16_9;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] dupes = GameObject.FindGameObjectsWithTag(startCanvasTag);

        // Remove existing canvas
        foreach(GameObject dupe in dupes)
        {
            Destroy(dupe);
        }

        // Add new canvas
        if (defaultAspectRatio == "4_3")
        {
            Instantiate(startCanvas4_3);
        }
        else if (defaultAspectRatio == "16_9")
        {
            Instantiate(startCanvas16_9);
        }
    }
}
