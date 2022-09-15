using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentTween
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

    public PacStudentTween(Vector3 startPos, Vector3 destPos, float startTime)
    {
        StartPos = startPos;
        DestPos = destPos;
        StartTime = startTime;
    }
}