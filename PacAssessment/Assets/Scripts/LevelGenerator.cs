using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    // Declare Variables
    private float width;
    private float height;
    private float scaleX = 26.0f;
    private float scaleY = 15.5f;
    private float scaleZ = 1.0f;
    private float spriteScaleX = 3.1f * 15.0f;
    private float spriteScaleY = 3.1f * 14.0f;
    private float startingXPos = 14.0f;
    private float startingYPos = -14.0f;
    private const float startingZPos = 0.0f;
    private int arrayIndex;

    public int tileArraySizeX;
    public int tileArraySizeY;

    private string[] quadrant = { "TopLeft", "TopRight", "BotLeft", "BotRight" };
    //private string[] legendString = { "Empty", "OutsideCorner", "OutsideWall", "InsideCorner", "InsideWall", "StandardPellet", "PowerPellet", "TJunction" };

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

    // PacStudent
    [SerializeField] private PacStudent pacStudent;

    // Ghost
    [SerializeField] private Ghost ghost;

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

    private Map map;

    // Start is called before the first frame update
    void Start()
    {
        reset();
        calculateMaths();
        createLevel();

        // Set up pacStudent
        pacStudent.SetInitialScale(tileArraySizeX, tileArraySizeY);
        pacStudent.SetMap(map);
        pacStudent.Setup();

        // Set up ghosts
        ghost.SetInitialScale(tileArraySizeX, tileArraySizeY);
        ghost.SetMap(map);
        ghost.Setup();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Create level
    /// </summary>
    private void createLevel()
    {
        map = gameObject.AddComponent<Map>();
        map.mapItems = new MapItems[(tileArraySizeX * tileArraySizeY * 4) - (2 * tileArraySizeY)];

        // Top Left Section
        generateGridMap(startingXPos - scaleX, startingYPos + scaleY, startingZPos, -scaleX, scaleY, quadrant[0]);

        // Top Right Section
        generateGridMap(startingXPos, startingYPos + scaleY, startingZPos, scaleX, scaleY, quadrant[1]);

        // Bottom Left Section
        generateGridMap(startingXPos - scaleX, startingYPos, startingZPos, -scaleX, -scaleY, quadrant[2]);

        // Bottom Right Section
        generateGridMap(startingXPos, startingYPos, startingZPos, scaleX, -scaleY, quadrant[3]);

        // Rotate and flip sprites to face the correct way
        foreach (MapItems mItem in map.mapItems)
        {
            if (mItem.Type == Legend.OutsideCorner)
            {
                map.RotateAndFlipOutsideCornerWall(mItem);
            }
            else if (mItem.Type == Legend.OutsideWall)
            {
                map.RotateAndFlipOutsideWall(mItem);
            }
            else if (mItem.Type == Legend.InsideWall)
            {
                map.RotateAndFlipInsideWall(mItem);
            }
            else if (mItem.Type == Legend.TJunction)
            {
                map.RotateAndFlipTJunctionWall(mItem);
            }
        }

        foreach(MapItems mItem in map.mapItems)
        {
            if (mItem.Type == Legend.InsideCorner)
            {
                map.RotateAndFlipInsideCornerWall(mItem);
            }
        }
    }

    /// <summary>
    /// Procedurally generate grid map
    /// </summary>
    /// <param name="startingXPos">The x position of the first tile</param>
    /// <param name="startingYPos">The y position of the first tile</param>
    /// <param name="startingZPos">The z position of the first tile</param>
    /// <param name="scaleX">The x scale for the tile</param>
    /// <param name="scaleY">The y scale for the tile</param>
    /// <param name="quadrant">What quadrant the gameobject is in</param>
    private void generateGridMap(float startingXPos, float startingYPos, float startingZPos, float scaleX, float scaleY, string quadrant)
    {
        float xPos = startingXPos;
        float yPos = startingYPos;
        float zPos = startingZPos;

        string[] neighbourPositions;

        GameObject newTileGO;
        GameObject newSpriteGO;

        // If in bottom quadrant, remove the last line of array
        int isInBotQuadrant;

        if (quadrant == this.quadrant[0] || quadrant == this.quadrant[1])
        {
            isInBotQuadrant = 0;
        }
        else
        {
            isInBotQuadrant = 1;
        }

        for (int i = tileArraySizeX - 1 - isInBotQuadrant; i >= 0; i--)
        {
            for (int j = tileArraySizeY - 1; j >= 0; j--)
            {
                newTileGO = createSquare(xPos, yPos, zPos);
                newTileGO.transform.localPosition = new Vector3(xPos, yPos, zPos);
                newTileGO.transform.localScale = new Vector3(width, height, scaleZ);
                newTileGO.name = quadrant + "_" + i + "_" + j;
                newSpriteGO = addSprite(levelMap[i, j], newTileGO);

                neighbourPositions = getNeighbourPositions(i, j, quadrant);
                map.mapItems[arrayIndex] = new MapItems(newTileGO, newSpriteGO, arrayIndex, (Legend)levelMap[i, j], neighbourPositions);

                arrayIndex++;
                xPos += scaleX;
            }
            xPos = startingXPos;
            yPos += scaleY;
        }
    }

    /// <summary>
    /// Create square gameobject tile and add to scene
    /// </summary>
    /// <param name="xPos">The x position of the gameobject</param>
    /// <param name="yPos">The y position of the gameobject</param>
    /// <param name="zPos">The z position of the gameobject</param>
    /// <returns>Square tile gameobject</returns>
    private GameObject createSquare(float xPos, float yPos, float zPos)
    {
        return Instantiate(squareTileGO, new Vector3(xPos, yPos, zPos), Quaternion.identity, gameObject.transform);
    }

    /// <summary>
    /// Add sprite to the parent square gameobject
    /// </summary>
    /// <param name="value">Determine what sprite to assign to gameobject</param>
    /// <param name="parent">The parent gameobject the sprite is assigned to</param>
    /// <returns>The gameobject of the sprite</returns>
    private GameObject addSprite(int value, GameObject parent)
    {
        GameObject newSpriteGO = new GameObject("Sprite");
        newSpriteGO.transform.parent = parent.transform;
        newSpriteGO.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        newSpriteGO.transform.localScale = new Vector3(spriteScaleX, spriteScaleY, 1.0f);
        SpriteRenderer rend = newSpriteGO.AddComponent<SpriteRenderer>();
        Animator animator = newSpriteGO.AddComponent<Animator>();

        switch (value)
        {
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

        return newSpriteGO;
    }

    /// <summary>
    /// Get neighbouring positions of a given position on the map
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="quadrant"></param>
    /// <returns>Array of neighbour positions</returns>
    private string[] getNeighbourPositions(float x, float y, string quadrant)
    {
        /*
         * let neighbourPositions[0] = left side of given position
         * neighbourPositions[1] = right side of given position
         * neighbourPositions[2] = top of the given position
         * neighbourPositions[3] = bottom of the given position
         */
        string[] neighbourPositions = new string[4];

        // TopLeft quadrant
        if (quadrant == this.quadrant[0])
        {

            if (x < tileArraySizeX - 1)
            {
                neighbourPositions[3] = quadrant + "_" + (x + 1) + "_" + y;
            }
            else
            {
                neighbourPositions[3] = this.quadrant[2] + "_" + (x - 1) + "_" + y;
            }

            if (x > 0)
            {
                neighbourPositions[2] = quadrant + "_" + (x - 1) + "_" + y;
            }
            else
            {
                neighbourPositions[2] = "Out of bounds";
            }

            if (y < tileArraySizeY - 1)
            {
                neighbourPositions[1] = quadrant + "_" + x + "_" + (y + 1);
            }
            else
            {
                neighbourPositions[1] = this.quadrant[1] + "_" + x + "_" + y;
            }

            if (y > 0)
            {
                neighbourPositions[0] = quadrant + "_" + x + "_" + (y - 1);
            }
            else
            {
                if (x == tileArraySizeX - 1)
                {
                    neighbourPositions[0] = this.quadrant[1] + "_" + x + "_" + y;
                }
                else
                {
                    neighbourPositions[0] = "Out of bounds";
                }
            }

        }

        // TopRight quadrant
        else if (quadrant == this.quadrant[1])
        {

            if (x < tileArraySizeX - 1)
            {
                neighbourPositions[3] = quadrant + "_" + (x + 1) + "_" + y;
            }
            else
            {
                neighbourPositions[3] = this.quadrant[3] + "_" + (x - 1) + "_" + y;
            }

            if (x > 0)
            {
                neighbourPositions[2] = quadrant + "_" + (x - 1) + "_" + y;
            }
            else
            {
                neighbourPositions[2] = "Out of bounds";
            }

            if (y < tileArraySizeY - 1)
            {
                neighbourPositions[0] = quadrant + "_" + x + "_" + (y + 1);
            }
            else
            {
                neighbourPositions[0] = this.quadrant[0] + "_" + x + "_" + y;
            }

            if (y > 0)
            {
                neighbourPositions[1] = quadrant + "_" + x + "_" + (y - 1);
            }
            else
            {
                if (x == tileArraySizeX - 1)
                {
                    neighbourPositions[1] = this.quadrant[0] + "_" + x + "_" + y;
                }
                else
                {
                    neighbourPositions[1] = "Out of bounds";
                }
            }

        }

        // BotLeft quadrant
        else if (quadrant == this.quadrant[2])
        {
            
            if (x < tileArraySizeX - 2)
            {
                neighbourPositions[2] = quadrant + "_" + (x + 1) + "_" + y;
            }
            else
            {
                neighbourPositions[2] = this.quadrant[0] + "_" + (x + 1) + "_" + y;
            }

            if (x > 0)
            {
                neighbourPositions[3] = quadrant + "_" + (x - 1) + "_" + y;
            }
            else
            {
                neighbourPositions[3] = "Out of bounds";
            }

            if (y < tileArraySizeY - 1)
            {
                neighbourPositions[1] = quadrant + "_" + x + "_" + (y + 1);
            }
            else
            {
                neighbourPositions[1] = this.quadrant[3] + "_" + x + "_" + y;
            }

            if (y > 0)
            {
                neighbourPositions[0] = quadrant + "_" + x + "_" + (y - 1);
            }
            else
            {
                if (x == tileArraySizeX - 1)
                {
                    neighbourPositions[0] = this.quadrant[3] + "_" + x + "_" + y;
                }
                else
                {
                    neighbourPositions[0] = "Out of bounds";
                }
            }

        }

        // BotRight quadrant
        else if (quadrant == this.quadrant[3])
        {
            
            if (x < tileArraySizeX - 2)
            {
                neighbourPositions[2] = quadrant + "_" + (x + 1) + "_" + y;
            }
            else
            {
                neighbourPositions[2] = this.quadrant[1] + "_" + (x + 1) + "_" + y;
            }

            if (x > 0)
            {
                neighbourPositions[3] = quadrant + "_" + (x - 1) + "_" + y;
            }
            else
            {
                neighbourPositions[3] = "Out of bounds";
            }

            if (y < tileArraySizeY - 1)
            {
                neighbourPositions[0] = quadrant + "_" + x + "_" + (y + 1);
            }
            else
            {
                neighbourPositions[0] = this.quadrant[2] + "_" + x + "_" + y;
            }

            if (y > 0)
            {
                neighbourPositions[1] = quadrant + "_" + x + "_" + (y - 1);
            }
            else
            {
                if (x == tileArraySizeX - 1)
                {
                    neighbourPositions[1] = this.quadrant[2] + "_" + x + "_" + y;
                }
                else
                {
                    neighbourPositions[1] = "Out of bounds";
                }
            }

        }
        return neighbourPositions;
    }

    /// <summary>
    /// Calculate the maths to account for various level map sizes
    /// </summary>
    private void calculateMaths()
    {
        arrayIndex = 0;

        // Get array size
        tileArraySizeX = levelMap.GetLength(0);
        tileArraySizeY = levelMap.GetLength(1);

        scaleX = scaleX * (15.0f / tileArraySizeY);
        scaleY = scaleY * (14.0f / tileArraySizeX);
        width = scaleX;
        height = scaleY;
        spriteScaleX /= tileArraySizeX;
        spriteScaleY /= tileArraySizeY;
    }

    /// <summary>
    /// Delete existing level from scene
    /// </summary>
    private void reset()
    {
        //Destroy(GameObject.Find("DisplayPacGameObjects"));
        Destroy(GameObject.Find("DisplayGridTopLeft"));
        Destroy(GameObject.Find("DisplayGridTopRight"));
        Destroy(GameObject.Find("DisplayGridBotLeft"));
        Destroy(GameObject.Find("DisplayGridBotRight"));

        GameObject.Find("CentreX").SetActive(false);
        GameObject.Find("CentreY").SetActive(false);
    }
}
