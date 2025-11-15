using UnityEngine;
using System.Collections;

public class NinoHorizontal : NinoBase
{
    [Header("Movimiento Horizontal")]
    [SerializeField] float speed = 15f;
    [SerializeField] float warningTime = 1.2f;
    public bool spawnFromLeft = true;

    [Header("Warning")]
    [SerializeField] WarningIndicator warningPrefab;

    [Header("Golpe por Overlap")]
    [SerializeField] LayerMask capaSanta;
    [SerializeField] float radioGolpe = 0.7f;
    [SerializeField] int dano = 1;
    [SerializeField] float duracionStun = 1f;

    Transform santa;
    Vector2 targetPos;
    bool launched;
    bool yaGolpeoSanta;

    protected override void Awake()
    {
        base.Awake();
        santa = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Start()
    {
        if (!santa) return;

        WarningIndicator warning = Instantiate(warningPrefab, santa.position, Quaternion.identity);
        warning.target = santa;
        warning.follow = true;
        warning.offset = spawnFromLeft ? new Vector2(-1.5f, 0f) : new Vector2(+1.5f, 0f);

        StartCoroutine(LaunchRoutine(warning));
    }

    IEnumerator LaunchRoutine(WarningIndicator warning)
    {
        yield return new WaitForSeconds(warningTime);

        targetPos = warning.transform.position;
        Destroy(warning.gameObject);

        launched = true;
    }

    private void Update()
    {
        if (!launched || muerto) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        DetectarGolpeSanta();

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            Destroy(gameObject);
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
            if (stun != null) stun.Stun(duracionStun);
        }

        yaGolpeoSanta = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioGolpe);
    }
}
