using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { private set; get; }
    #endregion

    public GameObject pressToStartText;
    public TMP_Text requiredEnergyText;

    Camera cam;

    void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    public void OnStartGame()
    {
        pressToStartText.SetActive(false);
        SetRequiredEnergyTextState(true);
    }

    #region Required Energy Text
    public void RequiredEnergyTextPosition(Vector2 worldPos)
    {
        requiredEnergyText.transform.position = cam.WorldToScreenPoint(worldPos);
    }

    public void SetRequiredEnergyTextState(bool state)
    {
        requiredEnergyText.gameObject.SetActive(state);
    }

    public void SetRequiredEnergyTextValue(int value)
    {
        requiredEnergyText.text = value.ToString();
    }
    #endregion
}
