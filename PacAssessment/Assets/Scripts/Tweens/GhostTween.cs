using UnityEngine;

public class GhostTween
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

    public GhostTween(Vector3 startPos, Vector3 destPos, float startTime)
    {
        StartPos = startPos;
        DestPos = destPos;
        StartTime = startTime;
    }
}