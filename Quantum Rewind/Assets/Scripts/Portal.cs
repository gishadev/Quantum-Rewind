using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject openedObject;
    public GameObject closedObject;

    Animator animator;
    CircleCollider2D circleCollider;

    void Awake()
    {
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Open()
    {
        animator.SetTrigger("Open");
        circleCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Anomaly"))
        {
            // If triggered by Original.
            if (other.GetComponent<Anomaly>().anomalyType == AnomalyType.Original)
            {
                GameManager.Instance.Win();
                circleCollider.enabled = false;
            }
        }
    }
}
