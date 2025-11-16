using UnityEngine;

namespace Unity.Cinemachine
{
    [RequireComponent(typeof(CinemachineBasicMultiChannelPerlin))]
    public class CinemachineShake : MonoBehaviour
    {
        [SerializeField] private float _intensity;
        [SerializeField] private float _speed;

        private CinemachineBasicMultiChannelPerlin _noise;

        private void Awake() => _noise = GetComponent<CinemachineBasicMultiChannelPerlin>();
        private void Update() => _noise.AmplitudeGain = _noise.FrequencyGain = Mathf.MoveTowards(_noise.FrequencyGain, 0, Time.deltaTime * _speed);

        public void Shake() => Shake(_intensity);
        public void Shake(float intensity) => _noise.AmplitudeGain = _noise.FrequencyGain = intensity;
    }
}