using UnityEngine;

public class GhostFourInstructions : MonoBehaviour
{
    public GhostInstruction[] Instructions = new GhostInstruction[]
    {
        new GhostInstruction(null, Direction.None),
        new GhostInstruction("BotRight_13_11", Direction.Left),
        new GhostInstruction("BotRight_13_13", Direction.Down),
        new GhostInstruction("BotRight_11_13", Direction.Right),
        new GhostInstruction("BotRight_11_9", Direction.Up),
        new GhostInstruction("TopRight_14_9", Direction.Right),
        new GhostInstruction("TopRight_14_6", Direction.Down),
        new GhostInstruction("BotRight_8_6", Direction.Right),
        new GhostInstruction("BotRight_8_1", Direction.Down),
        new GhostInstruction("BotRight_1_1", Direction.Left),
        new GhostInstruction("BotRight_1_12", Direction.Up),
        new GhostInstruction("BotRight_5_12", Direction.Left),
        new GhostInstruction("BotLeft_5_12", Direction.Down),
        new GhostInstruction("BotLeft_1_12", Direction.Left),
        new GhostInstruction("BotLeft_1_1", Direction.Up),
        new GhostInstruction("BotLeft_8_1", Direction.Right),
        new GhostInstruction("BotLeft_8_6", Direction.Up),
        new GhostInstruction("TopLeft_8_6", Direction.Left),
        new GhostInstruction("TopLeft_8_1", Direction.Up),
        new GhostInstruction("TopLeft_1_1", Direction.Right),
        new GhostInstruction("TopLeft_1_12", Direction.Down),
        new GhostInstruction("TopLeft_5_12", Direction.Right),
        new GhostInstruction("TopRight_5_12", Direction.Up),
        new GhostInstruction("TopRight_1_12", Direction.Right),
        new GhostInstruction("TopRight_1_1", Direction.Down),
        new GhostInstruction("TopRight_8_1", Direction.Left),
        new GhostInstruction("TopRight_8_6", Direction.Down),

        new GhostInstruction("TopLeft_1_6", Direction.Right),
        new GhostInstruction("TopRight_1_6", Direction.Right),
        new GhostInstruction("BotLeft_1_6", Direction.Left),
        new GhostInstruction("BotRight_1_6", Direction.Left),

        new GhostInstruction("TopLeft_5_1", Direction.Up),
        new GhostInstruction("TopRight_5_1", Direction.Down),
        new GhostInstruction("BotLeft_5_1", Direction.Up),
        new GhostInstruction("BotRight_5_1", Direction.Down),

        new GhostInstruction("TopLeft_5_9", Direction.Left),
        new GhostInstruction("TopRight_5_9", Direction.Right),
        new GhostInstruction("BotLeft_5_9", Direction.Left),
        new GhostInstruction("BotRight_5_9", Direction.Right),

        new GhostInstruction("TopLeft_8_9", Direction.Up),
        new GhostInstruction("TopRight_8_9", Direction.Up),
        new GhostInstruction("BotLeft_8_9", Direction.Down),
        new GhostInstruction("BotRight_8_9", Direction.Down),

        new GhostInstruction("TopLeft_8_12", Direction.Left),
        new GhostInstruction("TopRight_8_12", Direction.Right),
        new GhostInstruction("BotLeft_8_12", Direction.Left),
        new GhostInstruction("BotRight_8_12", Direction.Right),

        new GhostInstruction("TopLeft_11_9", Direction.Down),
        new GhostInstruction("TopRight_11_9", Direction.Down),
        new GhostInstruction("BotLeft_11_9", Direction.Up),
        new GhostInstruction("BotRight_11_9", Direction.Up),

        new GhostInstruction("TopLeft_14_0", Direction.Right),
        new GhostInstruction("TopRight_14_0", Direction.Left),
        new GhostInstruction("TopLeft_14_6", Direction.Up),
        new GhostInstruction("TopLeft_14_9", Direction.Left)
    };

    /// <summary>
    /// Get instruction
    /// </summary>
    /// <param name="positionName"></param>
    /// <returns></returns>
    public GhostInstruction GetInstruction(string positionName)
    {
        foreach (GhostInstruction instruction in Instructions)
        {
            if (positionName == instruction.PositionName) return instruction;
        }
        return Instructions[0];
    }
}