using UnityEngine;
using System.Collections;

public class NinoHorizontal : NinoBase
{
    [Header("Movimiento Horizontal")]
    [SerializeField] float speed = 15f;
    [SerializeField] float warningTime = 1.2f;

    [Header("Warning")]
    [SerializeField] WarningIndicator warningPrefab;

    [Header("Golpe por Overlap")]
    [SerializeField] LayerMask capaSanta;
    [SerializeField] float radioGolpe = 0.7f;
    [SerializeField] int dano = 1;
    [SerializeField] float duracionStun = 1f;

    [Header("Offscreen")]
    [SerializeField] float offscreenMargin = 0.2f;

    Transform santa;
    bool launched;
    bool yaGolpeoSanta;
    Vector2 moveDir;
    Camera mainCam;

    protected override void Awake()
    {
        base.Awake();
        santa = GameObject.FindGameObjectWithTag("Player")?.transform;
        mainCam = Camera.main;
    }

    void Start()
    {
        if (!santa) return;

        WarningIndicator warning = Instantiate(warningPrefab, santa.position, Quaternion.identity);
        warning.target = santa;
        warning.follow = true;

        bool fromLeft = transform.position.x < santa.position.x;

        warning.offset = fromLeft ? new Vector2(-1.5f, 0f) : new Vector2(+1.5f, 0f);

        moveDir = fromLeft ? Vector2.right : Vector2.left;

        StartCoroutine(LaunchRoutine(warning));
    }

    IEnumerator LaunchRoutine(WarningIndicator warning)
    {
        yield return new WaitForSeconds(warningTime);

        if (warning)
            Destroy(warning.gameObject);

        launched = true;
    }

    void Update()
    {
        if (!launched || muerto) return;

        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);

        DetectarGolpeSanta();

        if (mainCam != null)
        {
            Vector3 vp = mainCam.WorldToViewportPoint(transform.position);
            if (vp.x < -offscreenMargin || vp.x > 1f + offscreenMargin)
            {
                Destroy(gameObject);
            }
        }
    }

    void DetectarGolpeSanta()
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioGolpe);
    }
}
