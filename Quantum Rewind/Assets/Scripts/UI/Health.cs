using UnityEngine;

public class Health : MonoBehaviour
{
    public void Set(float value)
    {
        transform.localScale = new Vector3(Mathf.Clamp01(value),1f,1f);
    }
}
