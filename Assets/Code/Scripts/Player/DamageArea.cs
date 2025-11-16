using Player.Controller;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    private Core _player;

    private void Awake()
    {
        _player = GetComponentInParent<Core>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NinoBase kid))
        {
            kid.RecibirGolpe(_player.GetDirection);
            kid.TakeDamage(1);
            collision.attachedRigidbody?.AddForce(_player.GetDirection + Vector2.up * 25);
            print("aplicar daño");
        }
    }
}