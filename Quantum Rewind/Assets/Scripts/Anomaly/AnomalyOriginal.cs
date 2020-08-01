using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AnomalyOriginal : Anomaly
{
    public float speed;

    AnomalyDataWriter writer;

    void Awake()
    {
        writer = GetComponent<AnomalyDataWriter>();
    }

    void Start()
    {
        writer.StartDataWriting();
    }

    void Update()
    {
        // Movement.
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        transform.Translate(movementInput * speed * Time.deltaTime);

        ClampPosition();
    }

    void ClampPosition()
    {
        float bodyRadius = transform.localScale.x / 2f;

        float xPos = Mathf.Clamp(transform.position.x, WorldBoundaries.MinX + bodyRadius, WorldBoundaries.MaxX - bodyRadius);
        float yPos = Mathf.Clamp(transform.position.y, WorldBoundaries.MinY + bodyRadius, WorldBoundaries.MaxY - bodyRadius);
        transform.position = new Vector2(xPos, yPos);
    }
}
