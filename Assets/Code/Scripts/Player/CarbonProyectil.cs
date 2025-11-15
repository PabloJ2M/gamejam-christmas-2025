using UnityEngine;

public class CarbonProyectil : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float velocidad = 10f;
    [SerializeField] float vidaMax = 2f;

    [Header("Daño / Empuje")]
    [SerializeField] int danio = 1;
    [SerializeField] float fuerzaEmpujeExtraY = 0.3f;

    Rigidbody2D rb;
    float vidaRestante;
    Vector2 direccion = Vector2.right;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaRestante = vidaMax;
    }

    public void Inicializar(Vector2 dir)
    {
        direccion = dir.sqrMagnitude > 0.001f ? dir.normalized : Vector2.right;

        rb.linearVelocity = direccion * velocidad;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        vidaRestante -= Time.deltaTime;
        if (vidaRestante <= 0f)
            DestruirProyectil();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        IRecibeGolpe golpeable =
            other.GetComponent<IRecibeGolpe>() ??
            other.GetComponentInParent<IRecibeGolpe>();

        if (golpeable != null)
        {
            Vector2 dirGolpe = direccion;
            dirGolpe.y += fuerzaEmpujeExtraY;
            dirGolpe.Normalize();
            golpeable.RecibirGolpe(dirGolpe);
        }

        IDamageable d =
            other.GetComponent<IDamageable>() ??
            other.GetComponentInParent<IDamageable>();

        if (d != null)
        {
            d.TakeDamage(danio);
            DestruirProyectil();
            return;
        }

        if (!other.isTrigger)
            DestruirProyectil();
    }

    void DestruirProyectil()
    {
        var ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.transform.parent = null;
            ps.Stop();
            Destroy(ps.gameObject, 1f);
        }

        Destroy(gameObject);
    }
}