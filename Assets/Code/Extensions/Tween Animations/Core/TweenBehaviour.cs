using System.Threading.Tasks;

namespace UnityEngine.Animations
{
    [RequireComponent(typeof(TweenCore))]
    public abstract class TweenBehaviour<T> : MonoBehaviour
    {
        [SerializeField] protected AnimationCurve _animationCurve = new(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        protected float this[float t] => _animationCurve.Evaluate(t);

        protected TweenCore _tweenCore;
        protected int _tweenID = -1;

        protected virtual void Awake() => _tweenCore = GetComponent<TweenCore>();
        protected virtual void OnDisable() => _tweenCore.onPlayStatusChanged -= OnPerformePlay;
        protected virtual async void OnEnable()
        {
            _tweenCore.onPlayStatusChanged += OnPerformePlay;
            await Task.Yield();
            OnStart();
        }

        protected abstract void OnStart();
        protected abstract void OnPerformePlay(bool value);
        protected abstract void OnUpdate(T value);
        protected abstract void OnComplete();
    }
}