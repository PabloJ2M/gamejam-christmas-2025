using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] private InputActionReference _reference;
    [SerializeField] private UnityEvent<bool> _onPausedChanged;

    public bool IsPaused { get; set; }

    private void Start() => _reference.action.performed += SwitchPause;
    private void OnEnable() => _reference.action.Enable();
    private void OnDisable() => _reference.action.Disable();

    private void SwitchPause(InputAction.CallbackContext ctx)
    {
        if (!IsPaused) PauseOn();
        else PauseOff();
    }

    public void PauseOn()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        _onPausedChanged.Invoke(true);
    }
    public void PauseOff()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        _onPausedChanged.Invoke(false);
    }
}