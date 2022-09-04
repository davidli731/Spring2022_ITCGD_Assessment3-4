using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapItems
{
    public GameObject MapGO;
    public GameObject SpriteGO;
    public string Type;
    public string[]  NeighbourPositions;

    // Constructor
    public MapItems(
        GameObject mapGO,
        GameObject spriteGO,
        string type,
        string[] neighbourPositions)
    {
        MapGO = mapGO;
        SpriteGO = spriteGO;
        Type = type;
        NeighbourPositions = neighbourPositions;
    }
}

public class Map : MonoBehaviour
{
    public MapItems[] mapItems;
}
