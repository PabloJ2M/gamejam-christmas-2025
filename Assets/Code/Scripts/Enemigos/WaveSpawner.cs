using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] NinoMordedor prefabMordedor;
    [SerializeField] NinoMeteorito prefabMeteorito;
    [SerializeField] NinoHorizontal prefabHorizontal;

    [Header("Spawn Points")]
    [SerializeField] Transform[] mordedorSpawns;
    [SerializeField] Transform[] horizontalSpawns;

    [Header("Intervalos")]
    [SerializeField] float intervaloMordedor = 1f;
    [SerializeField] float intervaloPetardos = 3f;

    [Serializable]
    struct Wave
    {
        public int mordedores;
        public int verticales;
        public int horizontales;
    }

    Wave[] waves;

    void Start()
    {
        InitWaves();
        StartCoroutine(RunWaves());
    }

    void InitWaves()
    {
        waves = new Wave[10];

        waves[0] = new Wave { mordedores = 5, verticales = 2, horizontales = 1 };
        waves[1] = new Wave { mordedores = 6, verticales = 2, horizontales = 1 };
        waves[2] = new Wave { mordedores = 7, verticales = 3, horizontales = 2 };
        waves[3] = new Wave { mordedores = 8, verticales = 3, horizontales = 3 };
        waves[4] = new Wave { mordedores = 9, verticales = 4, horizontales = 3 };
        waves[5] = new Wave { mordedores = 10, verticales = 5, horizontales = 3 };
        waves[6] = new Wave { mordedores = 12, verticales = 5, horizontales = 4 };
        waves[7] = new Wave { mordedores = 14, verticales = 6, horizontales = 4 };
        waves[8] = new Wave { mordedores = 16, verticales = 7, horizontales = 5 };
        waves[9] = new Wave { mordedores = 18, verticales = 8, horizontales = 6 };
    }

    IEnumerator RunWaves()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            Wave w = waves[i];
            
            yield return StartCoroutine(SpawnMordedores(w.mordedores));

            yield return StartCoroutine(SpawnVerticales(w.verticales));

            yield return StartCoroutine(SpawnHorizontales(w.horizontales));

            yield return new WaitUntil(() => NinoBase.AliveCount == 0);

            yield return new WaitForSeconds(2f);
        }

    }

    IEnumerator SpawnMordedores(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            if (mordedorSpawns.Length == 0 || !prefabMordedor)
                yield break;

            Transform spawn = mordedorSpawns[Random.Range(0, mordedorSpawns.Length)];
            Instantiate(prefabMordedor, spawn.position, Quaternion.identity);

            yield return new WaitForSeconds(intervaloMordedor);
        }
    }

    IEnumerator SpawnVerticales(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            if (!prefabMeteorito)
                yield break;

            Instantiate(prefabMeteorito, Vector3.zero, Quaternion.identity);

            yield return new WaitForSeconds(intervaloPetardos);
        }
    }

    IEnumerator SpawnHorizontales(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            if (horizontalSpawns.Length == 0 || !prefabHorizontal)
                yield break;

            Transform spawn = horizontalSpawns[Random.Range(0, horizontalSpawns.Length)];

            Instantiate(prefabHorizontal, spawn.position, Quaternion.identity);

            yield return new WaitForSeconds(intervaloPetardos);
        }
    }
}