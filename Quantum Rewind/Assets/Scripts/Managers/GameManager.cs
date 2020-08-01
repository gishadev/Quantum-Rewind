using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { private set; get; }
    #endregion
    void Awake()
    {
        Instance = this;
    }
}
