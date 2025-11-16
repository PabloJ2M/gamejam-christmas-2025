using UnityEngine;

public class HealthPickupSpawner : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] HealthPickup pickupPrefab;

    [Header("Spawn Area")]
    [SerializeField] Vector2 areaMin = new Vector2(-10f, -3f);
    [SerializeField] Vector2 areaMax = new Vector2(10f, 3f);

    [Header("Configuracion")]
    [SerializeField] float spawnInterval = 8f;
    [Range(0f, 1f)]
    [SerializeField] float spawnChance = 0.4f;

    float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = spawnInterval;

        if (Random.value <= spawnChance)
        {
            SpawnPickup();
        }
    }

    private void SpawnPickup()
    {
        if (!pickupPrefab)
        {
            return;
        }

        float x = Random.Range(areaMin.x, areaMax.x);
        float y = Random.Range(areaMin.y, areaMax.y);
        Vector2 spawnPos = new Vector2(x, y);

        Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Vector3 center = (Vector3)((areaMin + areaMax) * 0.5f);
        Vector3 size = new Vector3(areaMax.x - areaMin.x, areaMax.y - areaMin.y, 0f);
        Gizmos.DrawCube(center, size);
    }
}

