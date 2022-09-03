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

    [SerializeField] private GameObject squareTileGO;

    // Sprites
    [SerializeField] private Sprite outsideCornerSprite;
    [SerializeField] private Sprite outsideWallSprite;
    [SerializeField] private Sprite insideCornerSprite;
    [SerializeField] private Sprite insideWallSprite;
    [SerializeField] private Sprite standardPelletSprite;
    [SerializeField] private Sprite powerPelletSprite;
    [SerializeField] private Sprite tJunctionSprite;

    // Animator
    [SerializeField] private RuntimeAnimatorController powerPelletController;

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

        // Top Left Section
        generateGridMap(startingXPos - scaleX, startingYPos + scaleY, startingZPos,  -scaleX, scaleY, 0);

        // Top Right Section
        generateGridMap(startingXPos, startingYPos + scaleY, startingZPos, scaleX, scaleY, 1);

        // Bottom Left Section
        generateGridMap(startingXPos - scaleX, startingYPos, startingZPos, -scaleX, -scaleY, 2);

        // Bottom Right Section
        generateGridMap(startingXPos, startingYPos, startingZPos, scaleX, -scaleY, 3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Procedurally generate grid map
    /// </summary>
    /// <param name="startingXPos">The x position of the first tile</param>
    /// <param name="startingYPos">The y position of the first tile</param>
    /// <param name="startingZPos">The z position of the first tile</param>
    /// <param name="scaleX">The x scale for the tile</param>
    /// <param name="scaleY">The y scale for the tile</param>
    private void generateGridMap(float startingXPos, float startingYPos, float startingZPos, float scaleX, float scaleY,  int quadrant)
    {
        float xPos = startingXPos;
        float yPos = startingYPos;
        float zPos = startingZPos;

        GameObject tile;

        switch (quadrant)
        {
            case 0:
                // Top left
                for (int i = tileArraySizeX - 1; i >= 0; i--)
                {
                    for (int j = tileArraySizeY - 1; j >= 0; j--)
                    {
                        tile = createSquare(xPos, yPos, zPos);
                        addSprite(levelMap[i, j], tile);
                        xPos += scaleX;
                    }
                    xPos = startingXPos;
                    yPos += scaleY;
                }
                break;

            case 1:
                // Top Right
                for (int i = 0; i < tileArraySizeX; i++)
                {
                    for (int j = tileArraySizeY; j > 0; j--)
                    {
                        tile = createSquare(xPos, yPos, zPos);
                        //addSprite(levelMap[i, j], tile);
                        xPos += scaleX;
                    }
                    xPos = startingXPos;
                    yPos += scaleY;
                }
                break;

            case 2:
                // Bottom Left
                for (int i = tileArraySizeX - 1; i >= 0; i--)
                {
                    for (int j = tileArraySizeY - 1; j >= 0; j--)
                    {
                        tile = createSquare(xPos, yPos, zPos);
                        addSprite(levelMap[i, j], tile);
                        xPos += scaleX;
                    }
                    xPos = startingXPos;
                    yPos += scaleY;
                }
                break;

            case 3:
                // Bottom Right
                for (int i = tileArraySizeX - 1; i >= 0; i--)
                {
                    for (int j = tileArraySizeY - 1; j >= 0; j--)
                    {
                        tile = createSquare(xPos, yPos, zPos);
                        addSprite(levelMap[i, j], tile);
                        xPos += scaleX;
                    }
                    xPos = startingXPos;
                    yPos += scaleY;
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Create square gameobject tile and add to scene
    /// </summary>
    /// <param name="xPos">The x position of the gameobject</param>
    /// <param name="yPos">The y position of the gameobject</param>
    /// <param name="zPos">The z position of the gameobject</param>
    /// <returns></returns>
    private GameObject createSquare(float xPos, float yPos, float zPos)
    {
        return Instantiate(squareTileGO, new Vector3(xPos, yPos, zPos), Quaternion.identity, gameObject.transform);
    }

    private void addSprite(int value, GameObject parent)
    {
        GameObject newSpriteGO = new GameObject("Sprite");
        newSpriteGO.transform.parent = parent.transform;
        newSpriteGO.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        newSpriteGO.transform.localScale = new Vector3(3.1f, 3.1f, 1.0f);
        SpriteRenderer rend = newSpriteGO.AddComponent<SpriteRenderer>();
        Animator animator = newSpriteGO.AddComponent<Animator>();

        switch (value)
        {
            case 0:
                break;
            case 1:
                rend.sprite = outsideCornerSprite;
                break;
            case 2:
                rend.sprite = outsideWallSprite;
                break;
            case 3:
                rend.sprite = insideCornerSprite;
                break;
            case 4:
                rend.sprite = insideWallSprite;
                break;
            case 5:
                rend.sprite = standardPelletSprite;
                break;
            case 6:
                rend.sprite = powerPelletSprite;
                animator.runtimeAnimatorController = powerPelletController;
                break;
            case 7:
                rend.sprite = tJunctionSprite;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Delete existing level from scene
    /// </summary>
    private void Reset()
    {
        //Destroy(GameObject.Find("DisplayPacGameObjects"));
        Destroy(GameObject.Find("DisplayGridTopLeft"));
        Destroy(GameObject.Find("DisplayGridTopRight"));
        Destroy(GameObject.Find("DisplayGridBotLeft"));
        Destroy(GameObject.Find("DisplayGridBotRight"));
    }
}
