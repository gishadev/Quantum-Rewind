using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    #region Singleton
    public static EnergyManager Instance { private set; get; }
    #endregion

    [Header("Instantiating")]
    public GameObject energyClusterPrefab;
    public Transform energyParent;
    [Space]
    [Tooltip("Part of all clusters at the scene.")]
    [Range(0, 100)] public int percentagePerBattery;
    public int EnergyPerBattery { private set; get; }

    private Vector2[] energyPoints;

    public Battery NowBattery
    {
        get
        {
            return SpawnManager.Instance.spawnpoints[GameManager.Instance.GameIteration].GetComponentInChildren<Battery>();
        }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetEnergyPoints();
    }

    void SetEnergyPoints()
    {
        EnergyCluster[] clusters = GetClusters();

        energyPoints = new Vector2[clusters.Length];
        for (int i = 0; i < clusters.Length; i++)
            energyPoints[i] = clusters[i].transform.position;

        EnergyPerBattery = Mathf.RoundToInt((percentagePerBattery / 100f) * clusters.Length);
    }

    public void InitEnergy()
    {
        ResetLastBatteryEnergy();

        DespawnEnergyClusters();
        SpawnEnergyClusters();
    }

    public EnergyCluster[] GetClusters()
    {
        return FindObjectsOfType<EnergyCluster>();
    }

    #region Energy Clusters
    void SpawnEnergyClusters()
    {
        foreach (Vector2 point in energyPoints)
            Instantiate(energyClusterPrefab, point, Quaternion.identity, energyParent);
    }

    void DespawnEnergyClusters()
    {
        EnergyCluster[] clusters = FindObjectsOfType<EnergyCluster>();
        foreach (EnergyCluster c in clusters)
            Destroy(c.gameObject);
    }
    #endregion

    void ResetLastBatteryEnergy()
    {
        NowBattery.Energy = 0;
    }
}
