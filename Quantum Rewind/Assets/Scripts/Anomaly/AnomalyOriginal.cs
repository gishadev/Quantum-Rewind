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

    bool isRecording = false;
    Vector2 mousePoint;
    Vector2 Direction { get { return (mousePoint - (Vector2)transform.position).normalized; } }
    float Magnitude { get { return Mathf.Clamp(Vector2.Distance(transform.position, mousePoint), 0, maxMagnitude); } }

    Health healthUI;
    Rigidbody2D rb;
    LineRenderer lr;
    Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        healthUI = FindObjectOfType<Health>();
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
            Time.timeScale = slowMotionScale;

            lr.enabled = true;
            isRecording = true;
        }

        if (isRecording)
        {
            lr.SetPosition(0, transform.position);
            mousePoint = cam.ScreenToWorldPoint(Input.mousePosition);
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

        healthUI.Set(LifeTime / maxLifeTime);
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
