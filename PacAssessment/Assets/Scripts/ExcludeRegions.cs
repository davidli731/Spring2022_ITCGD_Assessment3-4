using UnityEngine;

public class ExcludeRegions : MonoBehaviour
{
    public Positions[] ExcludePosition = new Positions[]
    {
        new Positions("TopLeft_12_4"),
        new Positions("TopLeft_12_3"),
        new Positions("TopLeft_12_2"),
        new Positions("TopLeft_12_1"),
        new Positions("TopLeft_12_0"),
        new Positions("TopLeft_11_4"),
        new Positions("TopLeft_11_3"),
        new Positions("TopLeft_11_2"),
        new Positions("TopLeft_11_1"),
        new Positions("TopLeft_11_0"),
        new Positions("TopLeft_10_4"),
        new Positions("TopLeft_10_3"),
        new Positions("TopLeft_10_2"),
        new Positions("TopLeft_10_1"),
        new Positions("TopLeft_10_0"),

        new Positions("TopRight_12_4"),
        new Positions("TopRight_12_3"),
        new Positions("TopRight_12_2"),
        new Positions("TopRight_12_1"),
        new Positions("TopRight_12_0"),
        new Positions("TopRight_11_4"),
        new Positions("TopRight_11_3"),
        new Positions("TopRight_11_2"),
        new Positions("TopRight_11_1"),
        new Positions("TopRight_11_0"),
        new Positions("TopRight_10_4"),
        new Positions("TopRight_10_3"),
        new Positions("TopRight_10_2"),
        new Positions("TopRight_10_1"),
        new Positions("TopRight_10_0"),

        new Positions("BotLeft_12_4"),
        new Positions("BotLeft_12_3"),
        new Positions("BotLeft_12_2"),
        new Positions("BotLeft_12_1"),
        new Positions("BotLeft_12_0"),
        new Positions("BotLeft_11_4"),
        new Positions("BotLeft_11_3"),
        new Positions("BotLeft_11_2"),
        new Positions("BotLeft_11_1"),
        new Positions("BotLeft_11_0"),
        new Positions("BotLeft_10_4"),
        new Positions("BotLeft_10_3"),
        new Positions("BotLeft_10_2"),
        new Positions("BotLeft_10_1"),
        new Positions("BotLeft_10_0"),

        new Positions("BotRight_12_4"),
        new Positions("BotRight_12_3"),
        new Positions("BotRight_12_2"),
        new Positions("BotRight_12_1"),
        new Positions("BotRight_12_0"),
        new Positions("BotRight_11_4"),
        new Positions("BotRight_11_3"),
        new Positions("BotRight_11_2"),
        new Positions("BotRight_11_1"),
        new Positions("BotRight_11_0"),
        new Positions("BotRight_10_4"),
        new Positions("BotRight_10_3"),
        new Positions("BotRight_10_2"),
        new Positions("BotRight_10_1"),
        new Positions("BotRight_10_0"),

        new Positions("TopLeft_3_3"),
        new Positions("TopLeft_3_4"),
        new Positions("TopRight_3_3"),
        new Positions("TopRight_3_4"),
        new Positions("BotLeft_3_3"),
        new Positions("BotLeft_3_4"),
        new Positions("BotRight_3_3"),
        new Positions("BotRight_3_4"),

        new Positions("TopLeft_3_8"),
        new Positions("TopLeft_3_9"),
        new Positions("TopLeft_3_10"),
        new Positions("TopRight_3_8"),
        new Positions("TopRight_3_9"),
        new Positions("TopRight_3_10"),
        new Positions("BotLeft_3_8"),
        new Positions("BotLeft_3_9"),
        new Positions("BotLeft_3_10"),
        new Positions("BotRight_3_8"),
        new Positions("BotRight_3_9"),
        new Positions("BotRight_3_10")
    };

    /// <summary>
    /// Check if given posName is in list
    /// </summary>
    /// <param name="posName"></param>
    /// <returns></returns>
    public bool IsIncluded(string posName)
    {
        foreach (Positions position in ExcludePosition)
        {
            if (position.PositionName == posName) return true;
        }
        return false;
    }
}