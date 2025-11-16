using Player.Controller;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    private Core _player;

    private void Awake()
    {
        _player = GetComponentInParent<Core>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IRecibeGolpe kid))
        {
            kid.RecibirGolpe(_player.GetDirection);
            collision.attachedRigidbody?.AddForce(_player.GetDirection + Vector2.up);
        }
    }
}