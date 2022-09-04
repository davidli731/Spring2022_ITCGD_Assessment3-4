using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapItems
{
    public GameObject MapGO;
    public GameObject SpriteGO;
    public int Index;
    public string Type;
    public string[]  NeighbourPositions;

    // Constructor
    public MapItems(
        GameObject mapGO,
        GameObject spriteGO,
        int index,
        string type,
        string[] neighbourPositions)
    {
        MapGO = mapGO;
        SpriteGO = spriteGO;
        Index = index;
        Type = type;
        NeighbourPositions = neighbourPositions;
    }
}

public class Map : MonoBehaviour
{
    public MapItems[] mapItems;

    /// <summary>
    /// Get index of gameobject from name
    /// </summary>
    /// <param name="name">Name of gameobject</param>
    /// <returns>index</returns>
    private int GetIndexFromString(string name)
    {
        foreach (MapItems item in mapItems)
        {
            if (name == item.MapGO.name)
            {
                return item.Index;
            }
        }
        return -1;
    }

    /// <summary>
    /// Check if item type is wall
    /// </summary>
    /// <param name="item"></param>
    /// <returns>Return true if item is wall, else false</returns>
    private bool IsOutsideWall(MapItems item)
    {
        return item.Type == "OutsideCorner" ||
            item.Type == "OutsideWall" ||
            item.Type == "TJunction";
    }

    private bool IsInsideWall(MapItems item)
    {
        return item.Type == "InsideCorner" ||
            item.Type == "InsideWall";
    }

    /// <summary>
    /// Rotate and flip outside corner wall
    /// </summary>
    /// <param name="item"></param>
    public void RotateAndFlipOutsideCornerWall(MapItems item)
    {
        if (GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[0])]) &&
            GetIndexFromString(item.NeighbourPositions[3]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[3])])
            )
        {
            item.SpriteGO.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GetIndexFromString(item.NeighbourPositions[2]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])]) &&
            GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[1])])
            )
        {
            item.SpriteGO.GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (GetIndexFromString(item.NeighbourPositions[2]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])]) &&
            GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[0])])
            )
        {
            item.SpriteGO.GetComponent<SpriteRenderer>().flipX = true;
            item.SpriteGO.GetComponent<SpriteRenderer>().flipY = true;
        }
    }

    /// <summary>
    /// Rotate and flip outside straight wall
    /// </summary>
    /// <param name="item"></param>
    public void RotateAndFlipOutsideWall(MapItems item)
    {
        if ((GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[0])]) &&
            GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[1])])) ||
            (GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[0])]) &&
            GetIndexFromString(item.NeighbourPositions[1]) == -1) ||
            (GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[1])]) &&
            GetIndexFromString(item.NeighbourPositions[0]) == -1)
            )
        {
            item.SpriteGO.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }
    }

    /// <summary>
    /// Rotate and flip inside corner wall
    /// </summary>
    /// <param name="item"></param>
    public void RotateAndFlipInsideCornerWall(MapItems item)
    {
        /*if (GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            mapItems[GetIndexFromString(item.NeighbourPositions[0])].Type == "InsideCorner" &&
            GetIndexFromString(item.NeighbourPositions[3]) > 0 &&
            IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[3])])
            )
        {
            item.SpriteGO.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            mapItems[GetIndexFromString(item.NeighbourPositions[1])].Type == "InsideCorner" &&
            GetIndexFromString(item.NeighbourPositions[2]) > 0 &&
            IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])])
            )
        {
            item.SpriteGO.GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            mapItems[GetIndexFromString(item.NeighbourPositions[0])].Type == "InsideCorner" &&
            GetIndexFromString(item.NeighbourPositions[2]) > 0 &&
            IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])])
            )
        {
            item.SpriteGO.GetComponent<SpriteRenderer>().flipX = true;
            item.SpriteGO.GetComponent<SpriteRenderer>().flipY = true;
        }*/
    }

    /// <summary>
    /// Rotate and flip inside straight wall
    /// </summary>
    /// <param name="item"></param>
    public void RotateAndFlipInsideWall(MapItems item)
    {
        if ((GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[0])]) &&
            GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[1])])) ||
            (GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[0])]) &&
            GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            mapItems[GetIndexFromString(item.NeighbourPositions[1])].Type == "Empty" &&
            !IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])]) &&
            !IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[3])])) ||
            (GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[1])]) &&
            GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            mapItems[GetIndexFromString(item.NeighbourPositions[0])].Type == "Empty" &&
            !IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])]) &&
            !IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[3])]))
            )
        {
            item.SpriteGO.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }
    }

    /// <summary>
    /// Rotate and flip t junction wall
    /// </summary>
    /// <param name="item"></param>
    public void RotateAndFlipTJunctionWall(MapItems item)
    {
        if (GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[0])]) &&
            GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[1])]) &&
            GetIndexFromString(item.NeighbourPositions[2]) > 0 &&
            IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])])
            )
        {
            item.SpriteGO.GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (
            GetIndexFromString(item.NeighbourPositions[2]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])]) &&
            GetIndexFromString(item.NeighbourPositions[3]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[3])]) &&
            GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[1])])
            )
        {
            item.SpriteGO.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }
        else if (
            GetIndexFromString(item.NeighbourPositions[2]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])]) &&
            GetIndexFromString(item.NeighbourPositions[3]) > 0 &&
            IsOutsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[3])]) &&
            GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            IsInsideWall(mapItems[GetIndexFromString(item.NeighbourPositions[0])])
            )
        {
            item.SpriteGO.transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
        };
    }
}
