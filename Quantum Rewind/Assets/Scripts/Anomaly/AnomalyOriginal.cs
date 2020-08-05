using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AnomalyOriginal : Anomaly
{
    [Header("Movement")]
    public float maxMagnitude;
    public float forceMultiplier;
    public float slowMotionScale;
    [Space]
    [SerializeField] private float maxLifeTime = 2f;
    public float LifeTime { private set; get; }

    public SpriteRenderer healthRenderer;

    bool isRecording = false;
    Vector2 firstMousePoint;
    Vector2 secondMousePoint;
    Vector2 Direction { get { return (secondMousePoint - firstMousePoint).normalized; } }
    float Magnitude { get { return Mathf.Clamp(Vector2.Distance(firstMousePoint, secondMousePoint), 0, maxMagnitude); } }

    Rigidbody2D rb;
    LineRenderer lr;
    Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        cam = Camera.main;
    }

    void Start()
    {
        ResetLifeTime();
        StartCoroutine(AnomalyDataWriter.DataWriting(this));
    }

    void Update()
    {
        //// Movement.
        //Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        //transform.Translate(movementInput * speed * Time.deltaTime);

        #region Movement
        if (Input.GetMouseButtonDown(0))
        {
            firstMousePoint = cam.ScreenToWorldPoint(Input.mousePosition);
            Time.timeScale = slowMotionScale;

            lr.enabled = true;
            isRecording = true;
        }

        if (isRecording)
        {
            lr.SetPosition(0, transform.position);
            secondMousePoint = cam.ScreenToWorldPoint(Input.mousePosition);
            lr.SetPosition(1, (Vector2)transform.position + (Direction * Magnitude));
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Magnitude > rb.velocity.magnitude / 2f)
                rb.velocity = Vector2.zero;
            rb.AddForce(Direction * Magnitude * forceMultiplier, ForceMode2D.Impulse);

            Time.timeScale = 1;

            lr.enabled = false;
            isRecording = false;
        }
        #endregion

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
