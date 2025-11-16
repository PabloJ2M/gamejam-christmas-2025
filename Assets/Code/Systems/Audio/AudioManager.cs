using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _sound, _effects;

    public void SetVolume(string name, float value) =>
        _mixer.SetFloat(name, Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);

    public void PlayOneShot(AudioClip clip)
    {
        _effects.PlayOneShot(clip);
    }
    public void SwipeSound(AudioClip clip)
    {
        _sound.clip = clip;
        _sound.Play();
    }
}