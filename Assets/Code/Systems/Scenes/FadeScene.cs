using System;
using UnityEngine.Animations;

namespace UnityEngine.SceneManagement
{
    public class FadeScene : TweenCanvasGroup
    {
        public Action onCompleteFade;

        protected override void OnStart()
        {
            _tweenCore.IsEnabled = onCompleteFade == null;
            OnUpdate(_tweenCore.IsEnabled ? 1f : 0f);
            _tweenCore.SwapTweenAnimation();
        }
        protected override void OnComplete()
        {
            base.OnComplete();
            onCompleteFade?.Invoke();
        }
    }
}