using UnityEngine;

public class GhostThreeInstructions : MonoBehaviour
{
    public GhostInstruction[] Instructions = new GhostInstruction[]
    {
        new GhostInstruction(null, Direction.None),
        new GhostInstruction("BotLeft_13_11", Direction.Right),
        new GhostInstruction("BotLeft_13_13", Direction.Down)
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