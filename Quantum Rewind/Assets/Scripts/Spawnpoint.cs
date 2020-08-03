using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    [HideInInspector] public Transform point { get { return this.transform; } }
    public Path pathData = new Path();
    public bool IsFormedPath { get { return pathData.isFormed; } }
}

