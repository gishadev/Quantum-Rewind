using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    public List<Vector2> tracePoints = new List<Vector2>();
    public bool isRetraced = false;
    public Path()
    {
    }
    public void RetracePath()
    {
        tracePoints.Reverse();
        isRetraced = true;
    }
}
