namespace UnityEngine.SceneManagement
{
    public class SceneController : SingletonBasic<SceneController>
    {
        [SerializeField] private FadeScene _fadePrefab;

        public void SwipeScene(string name)
        {
            Instantiate(_fadePrefab, transform).onCompleteFade += ChangeScene;
            void ChangeScene() => SceneManager.LoadSceneAsync(name);
        }
    }
}