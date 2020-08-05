using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Anomaly"))
        {
            // If triggered by Original.
            if (other.GetComponent<Anomaly>().anomalyType == AnomalyType.Original)
                GameManager.Instance.Win();
        }
    }
}
