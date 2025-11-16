using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    private AudioManager _manager;

    private void Awake() => _manager = AudioManager.Instance;
    
    public void PlayAudio(AudioClip clip) => _manager.PlayOneShot(clip);
}