using UnityEngine;

public class Battery : MonoBehaviour
{
    public SpriteRenderer indicatorsRenderer;
    public SpriteRenderer coreRenderer;
    public Transform coreMask;
    [Space]
    public Color gateColor;
    public Color chargedColor;

    private bool isCharged = false;
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
    public int EnergyRequired
    {
        get
        {
            int v = maxEnergy - Energy;
            
            if (v < 0)
                return 0;
            
                
            else
                return v;
        }
    }

    CircleCollider2D circleCollider;
    LineRenderer lr;
    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        lr = GetComponent<LineRenderer>();
    }

    #region Charging Up
    public void ChargeUp()
    {
        Energy++;
        UIManager.Instance.SetRequiredEnergyTextValue(EnergyRequired);

        if (EnergyRequired == 0)
            OnFullCharge();
    }

    void OnFullCharge()
    {
        isCharged = true;
        EnergyManager.Instance.DespawnEnergyClusters();
        UIManager.Instance.SetRequiredEnergyTextState(false);

        if (GameManager.Instance.IsFinalIteration)
        {
            GameManager.Instance.portal.Open();
            SetColor(chargedColor);
            ActivateLine();
        }
        else
            SetColor(gateColor);
    }

    void SetVisualEnergyValue(float value)
    {
        float clampedValue = Mathf.Clamp01(value);
        coreMask.localScale = Vector2.one * clampedValue;
    }

    void SetColor(Color color)
    {
        coreRenderer.color = color;
        indicatorsRenderer.color = color;
    }
    #endregion

    void ActivateLine()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, Vector2.zero);
        lr.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCharged && !GameManager.Instance.IsFinalIteration)
            if (other.CompareTag("Anomaly"))
            {
                if (other.GetComponent<Anomaly>().anomalyType == AnomalyType.Original)
                {
                    circleCollider.enabled = false;
                    GameManager.Instance.ReincarnateInNext();
                    SetColor(chargedColor);
                    ActivateLine();
                }
            }
    }
}
