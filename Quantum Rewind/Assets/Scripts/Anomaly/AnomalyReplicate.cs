using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AnomalyReplicate : Anomaly
    {
        public float speed;
        public Queue<Vector2> queueOfPoints = new Queue<Vector2>();

        void Start()
        {
            currentPath = spawnpoint.pathData;
            // Setting Queue.
            foreach (Vector2 point in currentPath.tracePoints)
                queueOfPoints.Enqueue(point);

            StartCoroutine(ReplicateMovement());
        }

        void Update()
        {
            // Rotating.
            transform.Rotate(Vector3.back, Time.deltaTime * 150f);
        }

        bool pathIsDone = false;
        IEnumerator ReplicateMovement()
        {
            while (queueOfPoints.Count > 0)
            {
                Vector2 nextPoint = queueOfPoints.Dequeue();

                StartCoroutine(MoveTowardsNextPoint(nextPoint));
                yield return new WaitUntil(() => pathIsDone);
                pathIsDone = false;
                yield return null;
            }
            // Destroying after passing all path.
            Destroy(gameObject);
        }

        IEnumerator MoveTowardsNextPoint(Vector2 point)
        {
            while (!pathIsDone)
            {
                if (Vector2.Distance(transform.position, point) == 0f)
                {
                    pathIsDone = true;
                    break;
                }

                float step = Time.deltaTime * speed;
                transform.position = Vector2.MoveTowards((Vector2)transform.position, point, step);
                yield return null;
            }
        }
    }


