using UnityEngine;

public class Battery : MonoBehaviour
{
    public Color gateColor = Color.yellow;
    private bool isOpened = false;
    public int Energy
    {
        get { return energy; }
        set
        {
            energy = value;
            SetVisualEnergyValue((float)Energy / maxEnergy);
        }
    }
    private int energy = 0;
    private int maxEnergy { get { return EnergyManager.Instance.EnergyPerBattery; } }

    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    #region Charging Up
    public void ChargeUp()
    {
        Energy++;

        if (Energy >= maxEnergy)
            OpenBatteryGate();
    }

    void OpenBatteryGate()
    {
        isOpened = true;
        spriteRenderer.color = gateColor;
    }

    void SetVisualEnergyValue(float value)
    {
        float clampedValue = Mathf.Clamp01(value);
        transform.localScale = Vector2.one * clampedValue;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpened)
            if (other.CompareTag("Anomaly"))
            {
                if (other.GetComponent<Anomaly>().anomalyType == AnomalyType.Original)
                {
                    circleCollider.enabled = false;
                    GameManager.Instance.ReincarnateInNext();
                }
            }
    }
}
