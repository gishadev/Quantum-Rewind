using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    public List<Vector2> tracePoints = new List<Vector2>();
    public bool isFormed = false;
    public Path()
    {
    }
    public void FormPath()
    {
        isFormed = true;
    }
}
