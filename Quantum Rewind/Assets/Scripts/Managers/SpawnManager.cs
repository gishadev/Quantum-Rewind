using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject originalAnomalyPrefab;
    public GameObject replicateAnomalyPrefab;
    [Space]
    public Spawnpoint[] spawnpoints;
    public Spawnpoint OriginalSpawnpoint { get { return spawnpoints[originalIndex]; } }
    public Anomaly OriginalAnomaly;

    int originalIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DespawnAll();

            if (OriginalSpawnpoint.path != null)
                FormReplicate();

            // Original Indexing.
            if (originalIndex + 1 < spawnpoints.Length)
                originalIndex++;
            else
                originalIndex = 0;

            SpawnOriginal();
            for (int i = 0; i < spawnpoints.Length; i++)
                if (spawnpoints[i].IsRetracedPath)
                    SpawnReplicate(spawnpoints[i]);
        }
    }

    void SpawnOriginal()
    {
        Vector2 position = OriginalSpawnpoint.point.position;
        Anomaly anomaly = Instantiate(originalAnomalyPrefab, position, Quaternion.identity).GetComponent<Anomaly>();

        OriginalAnomaly = anomaly;

        anomaly.spawnpoint = OriginalSpawnpoint;
    }

    void SpawnReplicate(Spawnpoint spawnpoint)
    {
        Vector2 position = spawnpoint.point.position;
        Anomaly anomaly = Instantiate(replicateAnomalyPrefab, position, Quaternion.identity).GetComponent<Anomaly>();

        anomaly.spawnpoint = spawnpoint;
    }

    void FormReplicate()
    {
        OriginalSpawnpoint.path.RetracePath();
    }

    void DespawnAll()
    {
        Anomaly[] anomalies = FindObjectsOfType<Anomaly>();

        if (anomalies.Length > 0)
            foreach (Anomaly a in anomalies)
                Destroy(a.gameObject);
    }
}

[System.Serializable]
public class Spawnpoint
{
    public Transform point;
    public Path path = new Path();
    public bool IsRetracedPath { get { return path.isRetraced; } }
}

