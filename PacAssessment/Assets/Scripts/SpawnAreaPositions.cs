using UnityEngine;
using UnityEngine.UIElements;

public class SpawnAreaPositions : MonoBehaviour
{
    public Positions[] SpawnPosition = new Positions[]
    {
        new Positions("TopLeft_14_13"),
        new Positions("TopLeft_14_12"),
        new Positions("TopLeft_14_11"),
        new Positions("TopLeft_13_13"),
        new Positions("TopLeft_13_12"),
        new Positions("TopLeft_13_11"),
        new Positions("TopLeft_12_13"),
        new Positions("TopRight_14_13"),
        new Positions("TopRight_14_12"),
        new Positions("TopRight_14_11"),
        new Positions("TopRight_13_13"),
        new Positions("TopRight_13_12"),
        new Positions("TopRight_13_11"),
        new Positions("TopRight_12_13"),
        new Positions("BotRight_14_13"),
        new Positions("BotRight_14_12"),
        new Positions("BotRight_14_11"),
        new Positions("BotRight_13_13"),
        new Positions("BotRight_13_12"),
        new Positions("BotRight_13_11"),
        new Positions("BotRight_12_13"),
        new Positions("BotLeft_14_13"),
        new Positions("BotLeft_14_12"),
        new Positions("BotLeft_14_11"),
        new Positions("BotLeft_13_13"),
        new Positions("BotLeft_13_12"),
        new Positions("BotLeft_13_11"),
        new Positions("BotLeft_12_13")
    };

    /// <summary>
    /// Check if given posName is in spawn
    /// </summary>
    /// <param name="posName"></param>
    /// <returns></returns>
    public bool IsInSpawn(string posName)
    {
        foreach (Positions position in SpawnPosition)
        {
            if (position.PositionName == posName) return true;
        }
        return false;
    }
}
