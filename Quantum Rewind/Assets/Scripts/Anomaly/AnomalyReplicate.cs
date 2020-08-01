using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnomalyReplicate : Anomaly
{
    public float speed;

    public Queue<Vector2> currentPath = new Queue<Vector2>();

    void Start()
    {
        foreach (Vector2 point in spawnpoint.path.tracePoints)
            currentPath.Enqueue(point);

        StartCoroutine(ReplicateMovement());
    }

    bool pathIsDone = false;

    IEnumerator ReplicateMovement()
    {
        while (currentPath.Count > 0)
        {
            Vector2 nextPoint = currentPath.Dequeue();

            StartCoroutine(MoveTowardsNextPoint(nextPoint));
            yield return new WaitUntil(() => pathIsDone);
            pathIsDone = false;
        }
    }

    IEnumerator MoveTowardsNextPoint(Vector2 point)
    {
        while (Vector2.Distance(transform.position, point) <= 0f)
        {
            transform.position = Vector2.MoveTowards(transform.position, point, Time.deltaTime * speed);
            pathIsDone = true;
            yield return null;
        }
    }
}
