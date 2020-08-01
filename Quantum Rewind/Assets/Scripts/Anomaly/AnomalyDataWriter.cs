using System.Collections;
using UnityEngine;

public class AnomalyDataWriter : MonoBehaviour
{
    Anomaly anomaly;

    public void StartDataWriting()
    {
        anomaly = GetComponent<Anomaly>();
        StartCoroutine(DataWriting());
    }

    IEnumerator DataWriting()
    {
        while (true)
        {
            anomaly.spawnpoint.path.tracePoints.Add(transform.position);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
