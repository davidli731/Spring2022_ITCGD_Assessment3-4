using UnityEngine;

public class GhostTwoInstructions : MonoBehaviour
{
    public GhostInstruction[] Instructions = new GhostInstruction[]
    {
        new GhostInstruction(null, Direction.None),
        new GhostInstruction("TopRight_13_11", Direction.Left),
        new GhostInstruction("TopRight_13_13", Direction.Up)
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