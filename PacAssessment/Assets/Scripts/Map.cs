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
    private bool IsWall(MapItems item)
    {
        return item.Type == "OutsideCorner" ||
            item.Type == "OutsideWall" ||
            item.Type == "InsideCorner" ||
            item.Type == "InsideWall" ||
            item.Type == "TJunction";
    }

    public void RotateTJunctionWalls(MapItems item)
    {
        if (GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            IsWall(mapItems[GetIndexFromString(item.NeighbourPositions[0])]) &&
            GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            IsWall(mapItems[GetIndexFromString(item.NeighbourPositions[1])]) &&
            GetIndexFromString(item.NeighbourPositions[2]) > 0 &&
            IsWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])])
            )
        {
            item.SpriteGO.GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (
            GetIndexFromString(item.NeighbourPositions[2]) > 0 &&
            IsWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])]) &&
            GetIndexFromString(item.NeighbourPositions[3]) > 0 &&
            IsWall(mapItems[GetIndexFromString(item.NeighbourPositions[3])]) &&
            GetIndexFromString(item.NeighbourPositions[1]) > 0 &&
            IsWall(mapItems[GetIndexFromString(item.NeighbourPositions[1])])
            )
        {
            item.SpriteGO.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }
        else if (
            GetIndexFromString(item.NeighbourPositions[2]) > 0 &&
            IsWall(mapItems[GetIndexFromString(item.NeighbourPositions[2])]) &&
            GetIndexFromString(item.NeighbourPositions[3]) > 0 &&
            IsWall(mapItems[GetIndexFromString(item.NeighbourPositions[3])]) &&
            GetIndexFromString(item.NeighbourPositions[0]) > 0 &&
            IsWall(mapItems[GetIndexFromString(item.NeighbourPositions[0])])
            )
        {
            item.SpriteGO.transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
        };
    }
}
