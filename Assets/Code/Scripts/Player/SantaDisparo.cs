using Player.Controller;
using UnityEngine;

public class SantaDisparo : MonoBehaviour
{
    [Header("Disparo")]
    
    [SerializeField] private CarbonProyectil prefabProyectil;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float tiempoEntreDisparos = 0.25f;

    private Core player;
    private float cooldown;

    private void Awake() => player = GetComponent<Core>();
    private void Update() => cooldown -= Time.deltaTime;

    private void OnAttack()
    {
        if (cooldown > 0f) return;
        player.Animator.SetTrigger("Shoot");
    }
    public void Disparar()
    {
        cooldown = tiempoEntreDisparos;

        CarbonProyectil nuevo = Instantiate(
            prefabProyectil,
            puntoDisparo.position,
            default
        );

        nuevo.Inicializar(Random.Range(-0.1f, 0.1f) * Vector2.up + player.GetDirection);
    }
}