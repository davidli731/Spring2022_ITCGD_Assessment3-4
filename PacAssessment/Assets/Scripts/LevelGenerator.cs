using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    // Declare Variables
    private const float scaleX = 1.25f;
    private const float scaleY = 0.65f;
    private const float startingXPos = 0.6f;
    private const float startingYPos = -0.35f;
    private const float startingZPos = 0.0f;
    private int tileArraySizeX;
    private int tileArraySizeY;

    [SerializeField] private GameObject tile;

    /* Level map index
    * 0 – Empty (do not put anything here, no sprite needed)
    * 1 - Outside corner (double lined corner piece in original game)
    * 2 - Outside wall (double line in original game)
    * 3 - Inside corner (single lined corner piece in original game)
    * 4 - Inside wall (single line in original game)
    * 5 – An empty space with a Standard pellet (see Visual Assets above)
    * 6 – An empty space with a Power pellet (see Visual Assets above)
    * 7 - A t junction piece for connecting with adjoining regions
    */
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
        Reset();

        // Get array size
        tileArraySizeX = levelMap.GetLength(0);
        tileArraySizeY = levelMap.GetLength(1);

        // Bottom Right Section
        generateGridMap(startingXPos, startingYPos, startingZPos, scaleX, -scaleY);

        // Bottom Left Section
        generateGridMap(startingXPos - scaleX, startingYPos, startingZPos, -scaleX, -scaleY);

        // Top Right Section
        generateGridMap(startingXPos, startingYPos + scaleY, startingZPos, scaleX, scaleY);

        // Top Left Section
        generateGridMap(startingXPos - scaleX, startingYPos + scaleY, startingZPos, -scaleX, scaleY);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Procedurally generate grid map
    /// </summary>
    /// <param name="startingXPos"></param>
    /// <param name="startingYPos"></param>
    /// <param name="startingZPos"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    private void generateGridMap(float startingXPos, float startingYPos, float startingZPos, float scaleX, float scaleY)
    {
        float xPos = startingXPos;
        float yPos = startingYPos;
        float zPos = startingZPos;

        for (int i = 0; i < tileArraySizeX; i++)
        {
            for (int j = 0; j < tileArraySizeY; j++)
            {
                Instantiate(tile, new Vector3(xPos, yPos, zPos), Quaternion.identity, gameObject.transform);
                xPos += scaleX;
            }
            xPos = startingXPos;
            yPos += scaleY;
        }
    }

    /// <summary>
    /// Delete existing level from scene
    /// </summary>
    private void Reset()
    {
        Destroy(GameObject.Find("DisplayGridTopLeft"));
        Destroy(GameObject.Find("DisplayGridTopRight"));
        Destroy(GameObject.Find("DisplayGridBotLeft"));
        Destroy(GameObject.Find("DisplayGridBotRight"));
    }
}
