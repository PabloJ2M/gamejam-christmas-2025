using UnityEngine;
using UnityEngine.Events;

public class EnableDelay : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private UnityEvent _onCompleteDelay;

    private void OnEnable() => Invoke(nameof(Callback), _delay);
    private void OnDisable() => CancelInvoke();

    private void Callback() => _onCompleteDelay.Invoke();
}