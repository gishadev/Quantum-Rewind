using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { private set; get; }
    #endregion
    public int GameIteration { private set; get; }
    public bool isPlaying = false;

    UIManager uiManager;
    SpawnManager spawnManager;
    EnergyManager energyManager;

    void Awake()
    {
        Instance = this;
        GameIteration = -1;
    }

    void Start()
    {
        uiManager = UIManager.Instance;
        spawnManager = SpawnManager.Instance;
        energyManager = EnergyManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);

        if (!isPlaying)
            if (Input.anyKeyDown)
                StartGame();
    }

    void StartGame()
    {
        isPlaying = true;
        ReincarnateInNext();
        uiManager.OnStartGame();
    }

    public void Win()
    {
        Debug.Log("Win!");
        SceneManager.LoadScene(0);
    }

    public void Lose()
    {
        Debug.Log("Lose!");
        SceneManager.LoadScene(0);
    }

    #region Reincarnations
    public void ReincarnateInNext()
    {
        NextIteration();
        if (IsOutOfIterations())
            return;

        energyManager.InitEnergy();
        spawnManager.SpawnAnomalies(true);
    }

    public void ReincarnateInCurrent()
    {
        energyManager.InitEnergy();
        spawnManager.SpawnAnomalies(false);
    }
    #endregion

    #region Iterations
    public void NextIteration()
    {
        GameIteration++;
        Debug.Log(GameIteration);
        if (IsOutOfIterations())
            Win();
    }
    public bool IsOutOfIterations()
    {
        return GameIteration >= spawnManager.spawnpoints.Length;
    }
    #endregion
}
