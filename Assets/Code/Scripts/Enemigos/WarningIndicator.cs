using UnityEngine;

public class WarningIndicator : MonoBehaviour
{
    public Transform target;
    public bool follow = true;

    public Vector2 offset; 

    private void Update()
    {
        if (!follow || target == null) return;

        transform.position = (Vector2)target.position + offset;
    }
}
