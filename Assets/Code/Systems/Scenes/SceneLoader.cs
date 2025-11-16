using UnityEngine.UI;

namespace UnityEngine.SceneManagement
{
    [RequireComponent(typeof(Button))]
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField, Scene] private string _sceneName;

        private void Awake() => GetComponent<Button>().onClick.AddListener(SwipeScene);

        private void SwipeScene() => SceneController.Instance.SwipeScene(_sceneName);
    }
}