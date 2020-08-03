using System.Collections.Generic;
using UnityEngine;

public class Anomaly : MonoBehaviour
{
    public Path currentPath = new Path();
    public Spawnpoint spawnpoint;

    [HideInInspector]
    public AnomalyType anomalyType
    {
        get
        {
            if (spawnpoint.pathData.isFormed)
                return AnomalyType.Replicate;
            else
                return AnomalyType.Original;
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}

public enum AnomalyType
{
    Original,
    Replicate
}
