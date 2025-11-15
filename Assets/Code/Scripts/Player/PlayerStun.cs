using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStun : MonoBehaviour
{
    [Header("Stun")]
    [SerializeField] float defaultStunDuration = 1f;

    [SerializeField] PlayerInput playerInput;
    [SerializeField] MonoBehaviour[] extraComponentsToDisable;

    bool stunned;
    float stunTimer;

    public bool IsStunned => stunned;
    public System.Action OnStunFinished;

    void Awake()
    {
        if (!playerInput)
            playerInput = GetComponent<PlayerInput>();
    }

    public void Stun(float duration = -1f)
    {
        if (duration <= 0f)
            duration = defaultStunDuration;

        stunTimer = duration;

        if (stunned) return;

        stunned = true;

        if (playerInput)
            playerInput.DeactivateInput();

        SetExtraComponentsEnabled(false);

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (!stunned) return;

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            stunned = false;

            if (playerInput)
                playerInput.ActivateInput();

            SetExtraComponentsEnabled(true);

            OnStunFinished?.Invoke();
        }
    }

    private void SetExtraComponentsEnabled(bool value)
    {
        foreach (var c in extraComponentsToDisable)
            if (c != null)
                c.enabled = value;
    }
}
