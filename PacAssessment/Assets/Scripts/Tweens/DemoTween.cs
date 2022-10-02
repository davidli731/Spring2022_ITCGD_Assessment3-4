using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTween
{
    public Vector3 StartPos
    {
        get;
        private set;
    }

    public Vector3 DestPos
    {
        get;
        private set;
    }

    public float StartTime
    {
        get;
        private set;
    }

    public float Duration
    {
        get;
        private set;
    }

    public Direction Direction
    {
        get;
        private set;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="destPos"></param>
    /// <param name="startTime"></param>
    /// <param name="duration"></param>
    /// <param name="direction"></param>
    public DemoTween(
        Vector3 startPos,
        Vector3 destPos,
        float startTime,
        float duration,
        Direction direction
        )
    {
        StartPos = startPos;
        DestPos = destPos;
        StartTime = startTime;
        Duration = duration;
        Direction = direction;
    }
}