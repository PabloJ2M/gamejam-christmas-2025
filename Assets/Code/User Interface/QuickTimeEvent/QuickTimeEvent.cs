using PrimeTween;
using UnityEngine.Events;

namespace UnityEngine.InputSystem
{
    public class QuickTimeEvent : MonoBehaviour
    {
        [SerializeField] private TweenSettings _startAnimation;

        [SerializeField] private InputActionReference _inputs;
        [SerializeField, Range(0f, 10f)] private float _duration;
        [SerializeField, Range(0f, 0.2f)] private float _hitPerTick;
        [SerializeField, Range(0f, 0.5f)] private float _tickRate;

        [SerializeField] private UnityEvent<bool> _onCompleteEvent;

        private ProgressBar _progress;
        private float _fillTarget, _pressIndex;
        private bool _isPlaying;

        private void Awake() => _progress = GetComponent<ProgressBar>();
        private void Start() => _inputs.action.performed += OnPressed;

        private void OnEnable() => Tween.Custom(startValue: 0f, endValue: 0.5f, _startAnimation, OnUpdateValue).OnComplete(OnEventStarted);
        private void Update()
        {
            if (_isPlaying)
                _progress.FillAmount = Mathf.Lerp(_progress.FillAmount, _fillTarget, Time.deltaTime * 10f);
        }

        private void OnUpdateValue(float value) => _fillTarget = _progress.FillAmount = value;
        private void OnPressed(InputAction.CallbackContext ctx)
        {
            float value = ctx.ReadValue<float>();
            if (value == 0 || _pressIndex == value) return;

            _pressIndex = value;
            _fillTarget += _hitPerTick;
        }
        private void OnPullBack() => _fillTarget -= _hitPerTick;

        private void OnEventStarted()
        {
            InvokeRepeating(nameof(OnPullBack), _tickRate, _tickRate);
            Invoke(nameof(OnEventCompleted), _duration);
            _inputs.action.Enable();
            _isPlaying = true;
        }
        private void OnEventCompleted()
        {
            _onCompleteEvent?.Invoke(_progress.FillAmount > 0.5f);
            _inputs.action.Disable();
            _isPlaying = false;
            CancelInvoke();
        }
    }
}