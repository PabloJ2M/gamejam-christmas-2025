using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] NinoHorizontal prefabHorizontal;
    [SerializeField] NinoMeteorito prefabMeteorito;

    [Header("Tiempos")]
    [SerializeField] float intervalo = 3f;

    [Header("Horizontal Settings")]
    [SerializeField] Transform spawnDerecha;
    [SerializeField] Transform spawnIzquierda;

    void Start()
    {
        StartCoroutine(TestRoutine());
    }

    IEnumerator TestRoutine()
    {
        while (true)
        {
            if (spawnDerecha != null)
            {
                NinoHorizontal n = Instantiate(prefabHorizontal, spawnDerecha.position, Quaternion.identity);
                n.spawnFromLeft = false;
            }

            yield return new WaitForSeconds(intervalo);

            if (spawnIzquierda != null)
            {
                NinoHorizontal n = Instantiate(prefabHorizontal, spawnIzquierda.position, Quaternion.identity);
                n.spawnFromLeft = true;
            }

            yield return new WaitForSeconds(intervalo);

            Instantiate(prefabMeteorito, Vector2.zero, Quaternion.identity);

            yield return new WaitForSeconds(intervalo);
        }
    }
}
