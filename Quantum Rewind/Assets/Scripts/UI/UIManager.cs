using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { private set; get; }
    #endregion

    public GameObject pressToStartText;

    void Awake()
    {
        Instance = this;
    }

    public void OnStartGame()
    {
        pressToStartText.SetActive(false);
    }
}
