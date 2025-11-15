using UnityEngine;

public interface IRecibeGolpe
{
    void RecibirGolpe(Vector2 direccionGolpe);
}

public interface IDamageable
{
    bool IsDead { get; }
    void TakeDamage(int amount);
}

public class NinoBase : MonoBehaviour, IDamageable, IRecibeGolpe
{
    [Header("Datos base")]
    [SerializeField] int vida = 1;
    [SerializeField] float fuerzaGolpe = 10f;
    [SerializeField] float torqueMuerte = 8f;
    [SerializeField] float tiempoDesaparecer = 1.5f;

    [Header("Referencias")]
    public Rigidbody2D rb;
    public Animator anim;

    public bool muerto;

    public bool IsDead => muerto;

    protected virtual void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!anim) anim = GetComponent<Animator>();
    }

    public virtual void TakeDamage(int amount)
    {
        if (muerto) return;

        vida -= amount;
        if (vida <= 0)
            Matar(Vector2.zero);
    }

    public virtual void RecibirGolpe(Vector2 direccionGolpe)
    {
        if (muerto) return;
        Matar(direccionGolpe);
    }

    protected virtual void Matar(Vector2 direccionGolpe)
    {
        muerto = true;

        if (anim)
            anim.SetTrigger("Die");

        var col = GetComponent<Collider2D>();
        if (col)
            col.enabled = false;

        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        Vector2 dir = direccionGolpe == Vector2.zero
            ? Vector2.up
            : direccionGolpe.normalized;

        dir.y = Mathf.Abs(dir.y) + 0.5f;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * fuerzaGolpe, ForceMode2D.Impulse);
        rb.AddTorque(torqueMuerte, ForceMode2D.Impulse);

        Destroy(gameObject, tiempoDesaparecer);
    }
}
