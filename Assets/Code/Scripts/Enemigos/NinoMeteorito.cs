using UnityEngine;
using System.Collections;

public class NinoMeteorito : NinoBase
{
    [Header("Caida")]
    [SerializeField] float fallSpeed = 18f;
    [SerializeField] float spawnOffsetY = 10f;
    [SerializeField] float warningTime = 1.2f;

    [Header("Warning")]
    [SerializeField] WarningIndicator warningPrefab;

    [Header("Golpe por Overlap")]
    [SerializeField] LayerMask capaSanta;
    [SerializeField] float radioGolpe = 0.7f;
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

        WarningIndicator warning = Instantiate(
            warningPrefab,
            santa.position,
            Quaternion.identity
        );

        warning.target = santa;
        warning.follow = true;
        warning.offset = new Vector2(0f, +2f);

        StartCoroutine(FallRoutine(warning));
    }

    IEnumerator FallRoutine(WarningIndicator warning)
    {
        yield return new WaitForSeconds(warningTime);

        float impactX = warning.transform.position.x;
        float impactY = santa ? santa.position.y : warning.transform.position.y - 2f;
        impactPos = new Vector2(impactX, impactY);

        Destroy(warning.gameObject);

        float spawnY = impactPos.y + spawnOffsetY;
        transform.position = new Vector2(impactPos.x, spawnY);

        if (sprite) sprite.enabled = true;
        if (hitCollider) hitCollider.enabled = true;

        falling = true;
    }

    private void Update()
    {
        if (!falling || muerto) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            impactPos,
            fallSpeed * Time.deltaTime
        );

        DetectarGolpeSanta();

        if (Vector2.Distance(transform.position, impactPos) < 0.05f)
        {
            DetectarGolpeSanta();
            StartCoroutine(BounceAndDie());
        }
    }

    private void DetectarGolpeSanta()
    {
        if (yaGolpeoSanta) return;

        Collider2D col = Physics2D.OverlapCircle(transform.position, radioGolpe, capaSanta);
        if (!col) return;

        Health health = col.GetComponent<Health>();
        if (health != null && !health.IsDead && !health.IsInvulnerable)
        {
            health.TakeDamage(dano);

            PlayerStun stun = col.GetComponent<PlayerStun>();
            if (stun != null)
                stun.Stun(duracionStun);
        }

        yaGolpeoSanta = true;
    }

    IEnumerator BounceAndDie()
    {
        falling = false;
        yaGolpeoSanta = true;

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
        Gizmos.DrawWireSphere(transform.position, radioGolpe);
    }
}
