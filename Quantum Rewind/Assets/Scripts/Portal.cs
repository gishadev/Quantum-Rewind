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

        EffectsEmitter.Emit("Big_Purple_Explosion", transform.position);

        AudioManager.Instance.PlaySFX("Portal_Open");
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
