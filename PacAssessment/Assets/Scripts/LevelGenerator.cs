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
    private int arrayIndex = 0;

    private string[] quadrant = {"TopLeft", "TopRight", "BotLeft", "BotRight"};
    private string[] legendString = { "Empty", "OutsideCorner", "OutsideWall", "InsideCorner", "InsideWall", "StandardPellet", "PowerPellet", "TJunction" };

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

    private Map map;

    // Start is called before the first frame update
    void Start()
    {
        Reset();

        // Get array size
        tileArraySizeX = levelMap.GetLength(0);
        tileArraySizeY = levelMap.GetLength(1);

        map = gameObject.AddComponent<Map>();
        map.mapItems = new MapItems[tileArraySizeX * tileArraySizeY];

        // Top Left Section
        generateGridMap(startingXPos - scaleX, startingYPos + scaleY, startingZPos,  -scaleX, scaleY, quadrant[0]);

        // Top Right Section
        generateGridMap(startingXPos, startingYPos + scaleY, startingZPos, scaleX, scaleY, quadrant[1]);

        // Bottom Left Section
        generateGridMap(startingXPos - scaleX, startingYPos, startingZPos, -scaleX, -scaleY, quadrant[2]);

        // Bottom Right Section
        generateGridMap(startingXPos, startingYPos, startingZPos, scaleX, -scaleY, quadrant[3]);
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
    /// <param name="quadrant">What quadrant the gameobject is in</param>
    private void generateGridMap(float startingXPos, float startingYPos, float startingZPos, float scaleX, float scaleY, string quadrant)
    {
        float xPos = startingXPos;
        float yPos = startingYPos;
        float zPos = startingZPos;

        string[] neighbourPositions = new string[4];

        GameObject newTileGO;
        GameObject newSpriteGO;

        for (int i = tileArraySizeX - 1; i >= 0; i--)
        {
            for (int j = tileArraySizeY - 1; j >= 0; j--)
            {
                newTileGO = createSquare(xPos, yPos, zPos);
                newTileGO.name = quadrant + "_" + i + "_" + j;
                newSpriteGO = addSprite(levelMap[i, j], newTileGO);

                neighbourPositions = getNeighbourPositions(i, j, quadrant);
                map.mapItems[arrayIndex] = new MapItems(newTileGO, newSpriteGO, legendString[levelMap[i, j]], neighbourPositions);

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
        newSpriteGO.transform.localScale = new Vector3(3.1f, 3.1f, 1.0f);
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
                neighbourPositions[3] = this.quadrant[2] + "_" + x + "_" + y;
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
                neighbourPositions[3] = this.quadrant[3] + "_" + x + "_" + y;
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
            
            if (x < tileArraySizeX - 1)
            {
                neighbourPositions[2] = quadrant + "_" + (x + 1) + "_" + y;
            }
            else
            {
                neighbourPositions[2] = this.quadrant[0] + "_" + x + "_" + y;
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
            
            if (x < tileArraySizeX - 1)
            {
                neighbourPositions[2] = quadrant + "_" + (x + 1) + "_" + y;
            }
            else
            {
                neighbourPositions[2] = this.quadrant[1] + "_" + x + "_" + y;
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
