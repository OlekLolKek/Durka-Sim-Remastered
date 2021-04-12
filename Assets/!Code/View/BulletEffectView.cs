using UnityEngine;


namespace DurkaSimRemastered
{
    public class BulletEffectView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _startAudioSource;
        
        public void Play()
        {
            _particleSystem.Play();
            _audioSource.Play();
        }

        public void PlayStart()
        {
            _startAudioSource.Play();
        }
    }
}