using UnityEngine;


namespace DurkaSimRemastered
{
    public class BulletEffectView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioSource _audioSource;
        
        public void Play()
        {
            _particleSystem.Play();
            _audioSource.Play();
        }
    }
}