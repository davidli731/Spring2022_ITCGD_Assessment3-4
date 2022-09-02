using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Declare Variables
    private const float startingXPos = -16.9f;
    private const float startingYPos = 9.4f;
    private const float startingZPos = 0.0f;
    private const float scaleX = 1.25f;
    private const float scaleY = 0.65f;
    private int tileArraySizeX;
    private int tileArraySizeY;
    private float xPos;
    private float yPos;
    private float zPos;

    [SerializeField] private GameObject tile;

    private int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    // Start is called before the first frame update
    void Start()
    {
        tileArraySizeX = levelMap.GetLength(0);
        tileArraySizeY = levelMap.GetLength(1);

        // Set initial position of x,y,z grid box (top left)
        xPos = startingXPos;
        yPos = startingYPos;
        zPos = startingZPos;

        // Procedurally generate grid map
        for (int i = 0; i < tileArraySizeX; i++)
        {
            for (int j = 0; j < tileArraySizeY; j++)
            {
                Instantiate(tile, new Vector3(xPos, yPos, zPos), Quaternion.identity, gameObject.transform);
                xPos += scaleX;
            }
            xPos = startingXPos;
            yPos -= scaleY;
        }

        /*foreach (int item in levelMap)
        {
            Debug.Log(item);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
