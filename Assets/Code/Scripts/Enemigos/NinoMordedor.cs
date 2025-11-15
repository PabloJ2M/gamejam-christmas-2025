using UnityEngine;

public class NinoMordedor : NinoBase
{
    [Header("Movimiento")]
    [SerializeField] float velocidad = 3f;
    [SerializeField] float distanciaDeteccionSanta = 20f;

    [Header("Esquivar disparos")]
    [SerializeField] LayerMask capaProyectiles;
    [SerializeField] float radioDeteccionProyectil = 1.5f;
    [SerializeField] float fuerzaSalto = 7f;
    [Range(0f, 1f)]
    [SerializeField] float probabilidadEsquivar = 0.4f;

    [Header("Ataque mordida")]
    [SerializeField] int danoMordida = 1;
    [SerializeField] float fuerzaEmpujeSanta = 8f;
    [SerializeField] float tiempoEntreMordidas = 0.7f;

    Transform santa;
    float ultimoTiempoMordida;
    int direccion = 1;   

    protected override void Awake()
    {
        base.Awake();

        GameObject santaObj = GameObject.FindGameObjectWithTag("Player");
        if (santaObj)
            santa = santaObj.transform;
    }

    void FixedUpdate()
    {
        if (muerto || !santa) return;

        float distancia = Vector2.Distance(transform.position, santa.position);

        if (distancia > distanciaDeteccionSanta)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            if (anim) anim.SetFloat("Speed", 0f);
            return;
        }

        float dirX = Mathf.Sign(santa.position.x - transform.position.x);
        direccion = dirX >= 0 ? 1 : -1;

        Vector2 vel = rb.linearVelocity;
        vel.x = direccion * velocidad;
        rb.linearVelocity = vel;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direccion;
        transform.localScale = scale;

        if (anim) anim.SetFloat("Speed", Mathf.Abs(vel.x));

        DetectarYEsquivarProyectil();
    }

    void DetectarYEsquivarProyectil()
    {
        if (rb.linearVelocity.y > 0.1f) return;

        Vector2 centro = (Vector2)transform.position + Vector2.right * direccion * radioDeteccionProyectil;

        Collider2D col = Physics2D.OverlapCircle(centro, radioDeteccionProyectil, capaProyectiles);
        if (!col) return;

        if (Random.value > probabilidadEsquivar) return;

        Vector2 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;

        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

        if (anim) anim.SetTrigger("Jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (muerto) return;
        if (Time.time - ultimoTiempoMordida < tiempoEntreMordidas) return;

        Health health = collision.collider.GetComponent<Health>();
        if (health == null || health.IsDead || health.IsInvulnerable) return;

        health.TakeDamage(danoMordida);
        ultimoTiempoMordida = Time.time;

        Rigidbody2D rbPlayer = collision.collider.attachedRigidbody;
        if (rbPlayer != null)
        {
            Vector2 dirEmpuje = (rbPlayer.position - rb.position).normalized;
            dirEmpuje.y = Mathf.Abs(dirEmpuje.y) + 0.2f;
            rbPlayer.AddForce(dirEmpuje * fuerzaEmpujeSanta, ForceMode2D.Impulse);
        }

        if (anim) anim.SetTrigger("Bite");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 centro = (Vector2)transform.position + Vector2.right * direccion * radioDeteccionProyectil;
        Gizmos.DrawWireSphere(centro, radioDeteccionProyectil);
    }
}
