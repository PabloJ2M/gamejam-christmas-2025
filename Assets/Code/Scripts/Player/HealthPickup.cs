using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] int amountToHeal = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.GetComponent<Health>();
        if (health == null || health.IsDead) return;

        health.Heal(amountToHeal);
        Destroy(gameObject);
    }
}
