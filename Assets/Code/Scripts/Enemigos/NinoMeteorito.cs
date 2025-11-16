using UnityEngine;
using System.Collections;

public class NinoMeteorito : NinoBase
{
    [Header("Fall")]
    [SerializeField] float fallSpeed = 7f;
    [SerializeField] float spawnOffsetY = 10f;
    [SerializeField] float warningTime = 1.6f;
    [SerializeField] float impactYOffset = -0.5f;

    [Header("Warning")]
    [SerializeField] WarningIndicator warningPrefab;
    [SerializeField] float warningOffsetY = 2.5f;

    [Header("Hit Santa")]
    [SerializeField] LayerMask capaSanta;
    [SerializeField] float radioGolpe = 0.9f;
    [SerializeField] int dano = 1;
    [SerializeField] float duracionStun = 1f;

    [Header("Rebote")]
    [SerializeField] float bounceHeight = 12f;
    [SerializeField] float bounceDuration = 0.35f;

    Transform santa;
    Vector2 impactPos;
    bool falling;
    bool yaGolpeoSanta;

    SpriteRenderer sprite;
    Collider2D hitCollider;

    protected override void Awake()
    {
        base.Awake();
        santa = GameObject.FindGameObjectWithTag("Player")?.transform;

        sprite = GetComponentInChildren<SpriteRenderer>();
        hitCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (!santa)
        {
            Destroy(gameObject);
            return;
        }

        if (sprite) sprite.enabled = false;
        if (hitCollider) hitCollider.enabled = false;

        StartCoroutine(WarningAndFall());
    }

    IEnumerator WarningAndFall()
    {
        WarningIndicator warning = Instantiate(
            warningPrefab,
            santa.position + Vector3.up * warningOffsetY,
            Quaternion.identity
        );

        float t = 0f;
        while (t < warningTime)
        {
            t += Time.deltaTime;

            if (santa)
            {
                Vector3 pos = santa.position;
                pos.y += warningOffsetY;
                warning.transform.position = pos;
            }

            yield return null;
        }

        float impactX = warning.transform.position.x;
        float impactY = santa ? santa.position.y + impactYOffset : warning.transform.position.y;
        impactPos = new Vector2(impactX, impactY);

        Destroy(warning.gameObject);

        float spawnY = impactPos.y + spawnOffsetY;
        transform.position = new Vector2(impactPos.x, spawnY);

        if (sprite) sprite.enabled = true;
        if (hitCollider) hitCollider.enabled = true;

        falling = true;
    }

    void Update()
    {
        if (!falling || muerto) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            impactPos,
            fallSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, impactPos) < 0.05f)
        {
            TryGolpearSantaEnImpacto();

            StartCoroutine(BounceAndDie());
        }
    }

    void TryGolpearSantaEnImpacto()
    {
        if (yaGolpeoSanta) return;

        Collider2D col = Physics2D.OverlapCircle(impactPos, radioGolpe, capaSanta);
        if (col)
        {
            Health health = col.GetComponent<Health>();
            if (health != null && !health.IsDead && !health.IsInvulnerable)
            {
                health.TakeDamage(dano);

                PlayerStun stun = col.GetComponent<PlayerStun>();
                if (stun != null)
                    stun.Stun(duracionStun);
            }
        }

        yaGolpeoSanta = true;
    }

    IEnumerator BounceAndDie()
    {
        falling = false;
        yaGolpeoSanta = true;
        if (hitCollider) hitCollider.enabled = false;

        Vector2 start = transform.position;
        Vector2 end = start + Vector2.up * bounceHeight;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / bounceDuration;
            transform.position = Vector2.Lerp(start, end, t);
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 pos = impactPos == Vector2.zero ? transform.position : (Vector3)impactPos;
        Gizmos.DrawWireSphere(pos, radioGolpe);
    }
}
