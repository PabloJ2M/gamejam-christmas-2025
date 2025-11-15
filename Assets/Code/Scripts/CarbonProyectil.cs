using UnityEngine;

public class CarbonProyectil : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float vidaMax = 2f;

    [Header("Daño / Empuje")]
    [SerializeField] private int danio = 1;
    [SerializeField] private float fuerzaEmpuje = 8f;

    private Rigidbody2D rb;
    private float vidaRestante;
    private Vector2 direccion = Vector2.right;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaRestante = vidaMax;
    }
    public void Inicializar(Vector2 dir)
    {
        direccion = dir.normalized;
        rb.linearVelocity = direccion * velocidad;
        float angulo = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
    }

    private void Update()
    {
        vidaRestante -= Time.deltaTime;
        if (vidaRestante <= 0f)
        {
            DestruirProyectil();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Nino"))
        {
            Rigidbody2D rbNino = other.attachedRigidbody;
            if (rbNino != null)
            {
                Vector2 dirEmpuje = direccion; 
                dirEmpuje.y += 0.3f;
                dirEmpuje.Normalize();

                rbNino.AddForce(dirEmpuje * fuerzaEmpuje, ForceMode2D.Impulse);
            }

            DestruirProyectil();
        }
        else if (!other.isTrigger)
        {
            DestruirProyectil();
        }
    }

    private void DestruirProyectil()
    {
        var ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.transform.parent = null;
            ps.Stop();
        }

        Destroy(gameObject);
    }
}