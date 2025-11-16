using System;
using UnityEngine.Animations;

namespace UnityEngine.SceneManagement
{
    public class FadeScene : TweenCanvasGroup
    {
        public string SceneName { private get; set; }
        public Action onCompleteFade;

        protected override void OnStart()
        {
            bool isFading = !string.IsNullOrEmpty(SceneName);
            OnUpdate(isFading ? 0f : 1f);
            _tweenCore.Play(isFading);
        }
        protected override void OnComplete()
        {
            base.OnComplete();
            onCompleteFade?.Invoke();
        }
    }
}