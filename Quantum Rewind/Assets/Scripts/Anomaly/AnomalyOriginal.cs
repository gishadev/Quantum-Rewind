using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AnomalyOriginal : Anomaly
{
    public float speed;
    [SerializeField] private float maxLifeTime = 2f;
    public float LifeTime { private set; get; }

    public SpriteRenderer healthRenderer;

    void Start()
    {
        ResetLifeTime();
        StartCoroutine(AnomalyDataWriter.DataWriting(this));
    }

    void Update()
    {
        // Movement.
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        transform.Translate(movementInput * speed * Time.deltaTime);

        ClampPosition();

        CalculateLifeTime();
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.Lose();
    }

    void ClampPosition()
    {
        float bodyRadius = transform.localScale.x / 2f;

        float xPos = Mathf.Clamp(transform.position.x, WorldBoundaries.MinX + bodyRadius, WorldBoundaries.MaxX - bodyRadius);
        float yPos = Mathf.Clamp(transform.position.y, WorldBoundaries.MinY + bodyRadius, WorldBoundaries.MaxY - bodyRadius);
        transform.position = new Vector2(xPos, yPos);
    }

    #region Life Time
    public void ResetLifeTime()
    {
        LifeTime = maxLifeTime;
    }

    void CalculateLifeTime()
    {
        if (LifeTime < 0)
            Die();
        else
            LifeTime -= Time.deltaTime;

        healthRenderer.transform.localScale = Vector2.one * (LifeTime / maxLifeTime);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If Original (Player) triggers replicate => Die()
        if (other.CompareTag("Anomaly"))
            Die();
    }
}

public static class AnomalyDataWriter
{
    public static IEnumerator DataWriting(Anomaly anomaly)
    {
        while (true)
        {
            anomaly.currentPath.tracePoints.Add(anomaly.transform.position);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
