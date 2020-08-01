using UnityEngine;

public static class WorldBoundaries
{
    public static float MaxX { get { return ((float)Screen.width / (float)Screen.height) * Camera.main.orthographicSize; } } 
    public static float MinX { get { return -MaxX; } }
    public static float MaxY { get {return Camera.main.orthographicSize; } }
    public static float MinY { get { return -MaxY; } }
}
