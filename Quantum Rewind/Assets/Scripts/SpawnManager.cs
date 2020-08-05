using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Singleton
    public static SpawnManager Instance { private set; get; }
    #endregion

    [Header("Prefabs")]
    public GameObject originalAnomalyPrefab;
    public GameObject replicateAnomalyPrefab;
    [Space]
    public Spawnpoint[] spawnpoints;
    public Spawnpoint OriginalSpawnpoint { get { return spawnpoints[OriginalIndex]; } }
    public Anomaly OriginalAnomaly { private set; get; }

    int OriginalIndex { get { return GameManager.Instance.GameIteration; } }

    void Awake()
    {
        Instance = this;
    }

    public void SpawnAnomalies(bool spawnNewReplicate)
    {
        if (OriginalAnomaly != null && spawnNewReplicate)
            SetNewPathData();
        DespawnAll();

        // Spawning.
        SpawnOriginal();
        for (int i = 0; i < spawnpoints.Length; i++)
            if (spawnpoints[i].IsFormedPath)
                SpawnReplicate(spawnpoints[i]);
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

    void SetNewPathData()
    {
        if (OriginalAnomaly.currentPath != null)
            OriginalAnomaly.spawnpoint.pathData = OriginalAnomaly.currentPath;

        OriginalAnomaly.spawnpoint.pathData.FormPath();
    }

    void DespawnAll()
    {
        Anomaly[] anomalies = FindObjectsOfType<Anomaly>();

        if (anomalies.Length > 0)
            foreach (Anomaly a in anomalies)
                Destroy(a.gameObject);
    }
}

