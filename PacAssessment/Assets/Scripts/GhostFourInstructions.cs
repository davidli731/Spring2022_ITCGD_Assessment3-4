using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GhostFourInstruction
{
    public string PositionName;
    public Direction Direction;

    // Constructor
    public GhostFourInstruction(
        string positionName,
        Direction direction)
    {
        PositionName = positionName;
        Direction = direction;
    }
}

public class GhostFourInstructions : MonoBehaviour
{
    public GhostFourInstruction[] Instructions = new GhostFourInstruction[]
    {
        new GhostFourInstruction(null, Direction.None),
        new GhostFourInstruction("BotRight_13_11", Direction.Left),
        new GhostFourInstruction("BotRight_13_13", Direction.Down),
        new GhostFourInstruction("BotRight_11_13", Direction.Right),
        new GhostFourInstruction("BotRight_11_9", Direction.Up),
        new GhostFourInstruction("TopRight_14_9", Direction.Right),
        new GhostFourInstruction("TopRight_14_6", Direction.Down),
        new GhostFourInstruction("BotRight_8_6", Direction.Right),
        new GhostFourInstruction("BotRight_8_1", Direction.Down),
        new GhostFourInstruction("BotRight_1_1", Direction.Left),
        new GhostFourInstruction("BotRight_1_12", Direction.Up),
        new GhostFourInstruction("BotRight_5_12", Direction.Left),
        new GhostFourInstruction("BotLeft_5_12", Direction.Down),
        new GhostFourInstruction("BotLeft_1_12", Direction.Left),
        new GhostFourInstruction("BotLeft_1_1", Direction.Up),
        new GhostFourInstruction("BotLeft_8_1", Direction.Right),
        new GhostFourInstruction("BotLeft_8_6", Direction.Up),
        new GhostFourInstruction("TopLeft_8_6", Direction.Left),
        new GhostFourInstruction("TopLeft_8_1", Direction.Up),
        new GhostFourInstruction("TopLeft_1_1", Direction.Right),
        new GhostFourInstruction("TopLeft_1_12", Direction.Down),
        new GhostFourInstruction("TopLeft_5_12", Direction.Right),
        new GhostFourInstruction("TopRight_5_12", Direction.Up),
        new GhostFourInstruction("TopRight_1_12", Direction.Right),
        new GhostFourInstruction("TopRight_1_1", Direction.Down),
        new GhostFourInstruction("TopRight_8_1", Direction.Left),
        new GhostFourInstruction("TopRight_8_6", Direction.Down)
    };

    /// <summary>
    /// Get instruction
    /// </summary>
    /// <param name="positionName"></param>
    /// <returns></returns>
    public GhostFourInstruction GetInstruction(string positionName)
    {
        foreach (GhostFourInstruction instruction in Instructions)
        {
            if (positionName == instruction.PositionName) return instruction;
        }
        return Instructions[0];
    }
}