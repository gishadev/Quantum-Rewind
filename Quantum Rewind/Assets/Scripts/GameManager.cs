using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { private set; get; }
    #endregion
    public int GameIteration { private set; get; }
    public bool IsFinalIteration { get { return GameIteration == spawnManager.spawnpoints.Length - 1; } }
    public bool isPlaying = false;

    public Portal portal;

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

        PostProcessingController.Instance.TriggerDepthOfField(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

        if (!isPlaying)
            if (Input.anyKeyDown)
                StartGame();
    }

    void StartGame()
    {
        isPlaying = true;

        PostProcessingController.Instance.TriggerDepthOfField(false);

        ReincarnateInNext();

        uiManager.OnStartGame();
        uiManager.RequiredEnergyTextPosition(energyManager.NowBattery.transform.position);
    }

    public void Win()
    {
        Debug.Log("Win!");

        int nextScene;
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        else
            nextScene = 1;

        SceneManager.LoadScene(nextScene);
    }

    public void Lose()
    {
        Debug.Log("Lose!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region Reincarnations
    public void ReincarnateInNext()
    {
        NextIteration();
        if (IsOutOfIterations())
            return;

        uiManager.RequiredEnergyTextPosition(energyManager.NowBattery.transform.position);
        uiManager.SetRequiredEnergyTextValue(energyManager.EnergyPerBattery);
        uiManager.SetRequiredEnergyTextState(true);

        AudioManager.Instance.PlaySFX("Start");

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
        //Debug.Log(GameIteration);
    }
    public bool IsOutOfIterations()
    {
        return GameIteration >= spawnManager.spawnpoints.Length;
    }
    #endregion
}
