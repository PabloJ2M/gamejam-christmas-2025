using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioSettings : MonoBehaviour
{
    [SerializeField] private string _parameter;

    private AudioManager _manager;
    private Slider _slider;

    private void Awake()
    {
        _manager = AudioManager.Instance;
        _slider = GetComponent<Slider>();
    }
    private void Start()
    {
        _slider.onValueChanged.AddListener(SetVolume);
        _slider.value = PlayerPrefs.GetFloat($"Audio_{_parameter}", 0.5f);
    }

    private void SetVolume(float value)
    {
        _manager?.SetVolume(_parameter, value);
        PlayerPrefs.SetFloat($"Audio_{_parameter}", value);
    }
}