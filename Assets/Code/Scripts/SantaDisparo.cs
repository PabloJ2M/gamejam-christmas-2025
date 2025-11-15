using UnityEngine;

public class SantaDisparo : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private CarbonProyectil prefabProyectil;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float tiempoEntreDisparos = 0.25f;

    private float cooldown;

    void Update()
    {
        cooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J) && cooldown <= 0f)
        {
            Disparar();
        }
    }

    void Disparar()
    {
        cooldown = tiempoEntreDisparos;

        float dirX = Mathf.Sign(transform.localScale.x);
        Vector2 direccion = dirX > 0 ? Vector2.right : Vector2.left;

        CarbonProyectil nuevo = Instantiate(
            prefabProyectil,
            puntoDisparo.position,
            Quaternion.identity
        );

        nuevo.Inicializar(direccion);

    }
}
