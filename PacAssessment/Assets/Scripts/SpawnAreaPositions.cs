using UnityEngine;

public struct SpawnAreaPosition
{
    public string PositionName;

    // Constructor
    public SpawnAreaPosition(string positionName)
    {
        PositionName = positionName;
    }
}

public class SpawnAreaPositions : MonoBehaviour
{
    public SpawnAreaPosition[] SpawnPosition = new SpawnAreaPosition[]
    {
        new SpawnAreaPosition("TopLeft_14_13"),
        new SpawnAreaPosition("TopLeft_14_12"),
        new SpawnAreaPosition("TopLeft_14_11"),
        new SpawnAreaPosition("TopLeft_13_13"),
        new SpawnAreaPosition("TopLeft_13_12"),
        new SpawnAreaPosition("TopLeft_13_11"),
        new SpawnAreaPosition("TopLeft_12_13"),
        new SpawnAreaPosition("TopRight_14_13"),
        new SpawnAreaPosition("TopRight_14_12"),
        new SpawnAreaPosition("TopRight_14_11"),
        new SpawnAreaPosition("TopRight_13_13"),
        new SpawnAreaPosition("TopRight_13_12"),
        new SpawnAreaPosition("TopRight_13_11"),
        new SpawnAreaPosition("TopRight_12_13"),
        new SpawnAreaPosition("BotRight_14_13"),
        new SpawnAreaPosition("BotRight_14_12"),
        new SpawnAreaPosition("BotRight_14_11"),
        new SpawnAreaPosition("BotRight_13_13"),
        new SpawnAreaPosition("BotRight_13_12"),
        new SpawnAreaPosition("BotRight_13_11"),
        new SpawnAreaPosition("BotRight_12_13"),
        new SpawnAreaPosition("BotLeft_14_13"),
        new SpawnAreaPosition("BotLeft_14_12"),
        new SpawnAreaPosition("BotLeft_14_11"),
        new SpawnAreaPosition("BotLeft_13_13"),
        new SpawnAreaPosition("BotLeft_13_12"),
        new SpawnAreaPosition("BotLeft_13_11"),
        new SpawnAreaPosition("BotLeft_12_13")
    };

    /// <summary>
    /// Check if given posName is in spawn
    /// </summary>
    /// <param name="posName"></param>
    /// <returns></returns>
    public bool IsInSpawn(string posName)
    {
        foreach (SpawnAreaPosition position in SpawnPosition)
        {
            if (position.PositionName == posName) return true;
        }
        return false;
    }
}
