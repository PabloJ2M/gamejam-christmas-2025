using Player.Controller;
using UnityEngine;

public class SantaDisparo : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private CarbonProyectil _prefabProyectil;
    [SerializeField] private Transform _puntoDisparo;
    [SerializeField] private float _tiempoEntreDisparos = 0.25f;
    [SerializeField] private float _knockbackForce = 1f;

    private Core _player;
    private float _cooldown;

    private void Awake() => _player = GetComponent<Core>();
    private void Update() => _cooldown -= Time.deltaTime;

    internal void OnAttack()
    {
        if (_cooldown > 0f) return;
        _player.Animator.SetTrigger("Shoot");
    }
    public void Disparar()
    {
        _cooldown = _tiempoEntreDisparos;
        _player.AddForceX(-_player.GetDirection.x * _knockbackForce);

        CarbonProyectil nuevo = Instantiate(
            _prefabProyectil,
            _puntoDisparo.position,
            default
        );

        nuevo.Inicializar(Random.Range(-0.1f, 0.1f) * Vector2.up + _player.GetDirection);
    }
}