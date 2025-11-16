using UnityEngine;
using Player.Controller;

public class AtackHandler : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private CarbonProyectil _prefabProyectil;
    [SerializeField] private Transform _puntoDisparo, _puntoAtaque;
    [SerializeField] private LayerMask _hitLayer;

    [SerializeField] private float _tiempoEntreDisparos = 0.25f, _tiempoEntrePatada = 1f;
    [SerializeField] private float _knockbackForce = 1f;

    private Core _player;
    private float _shootCooldown, _atackCooldown;

    private void Awake() => _player = GetComponent<Core>();
    private void Update()
    {
        _shootCooldown = Mathf.MoveTowards(_shootCooldown, 0, Time.deltaTime);
        _atackCooldown = Mathf.MoveTowards(_atackCooldown, 0, Time.deltaTime);
    }

    internal void OnAttack()
    {
        if (_atackCooldown > 0f) return;
        _player.Animator.SetTrigger("Attack");
    }
    internal void OnShoot()
    {
        if (_shootCooldown > 0f) return;
        _player.Animator.SetTrigger("Shoot");
    }

    public void Disparar()
    {
        _shootCooldown = _tiempoEntreDisparos;
        _player.AddForceX(-_player.GetDirection.x * _knockbackForce);

        CarbonProyectil nuevo = Instantiate(
            _prefabProyectil,
            _puntoDisparo.position,
            default
        );

        nuevo.Inicializar(Random.Range(-0.1f, 0.1f) * Vector2.up + (Vector2)_puntoDisparo.right);
    }
    public void Ataque()
    {
        _atackCooldown = _tiempoEntrePatada;
        _puntoAtaque.gameObject.SetActive(true);
        Invoke(nameof(HideKick), _atackCooldown);
    }
    private void HideKick()
    {
        _puntoAtaque.gameObject.SetActive(false);
    }
}