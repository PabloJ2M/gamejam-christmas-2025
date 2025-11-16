using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    [SerializeField] int vidaMax = 3;
    int vidaActual;

    [Header("Invulnerabilidad")]
    [SerializeField] float duracionInvulnerabilidad = 0.7f;
    bool invulnerable;
    float tiempoInvulnerableRestante;

    public bool IsDead => vidaActual <= 0;
    public bool IsInvulnerable => invulnerable;

    void Awake()
    {
        vidaActual = vidaMax;
    }

    private void Update()
    {
        if (invulnerable)
        {
            tiempoInvulnerableRestante -= Time.deltaTime;
            if (tiempoInvulnerableRestante <= 0f)
            {
                invulnerable = false;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;
        if (invulnerable) return;

        vidaActual -= amount;
        if (vidaActual < 0) vidaActual = 0;

        ActivarInvulnerabilidad();

        if (vidaActual <= 0)
        {
            Debug.Log("Moriste");
        }
    }

    public void Heal(int amount)
    {
        if (IsDead) return;

        vidaActual += amount;
        if (vidaActual > vidaMax) vidaActual = vidaMax;

    }

    private void ActivarInvulnerabilidad()
    {
        invulnerable = true;
        tiempoInvulnerableRestante = duracionInvulnerabilidad;
    }
}
